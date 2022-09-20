using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace AttributeValidation
{
    public class AttributeValidationResult
    {
        public enum ResultKind
        {
            Success,
            Failure,
            InternalError,
        }

        public ResultKind ValidationResultKind { get; set; }

        public Exception InternalError { get; set; }

        public List<InvalidAssetsEntry> Errors { get; set; }

        [JsonIgnore]
        private Dictionary<AssetToValidate, RecursiveAssetValidation> m_invalidAssets;

        [JsonIgnore]
        public IReadOnlyDictionary<AssetToValidate, RecursiveAssetValidation> InvalidAssets => m_invalidAssets;

        public static List<InvalidAssetsEntry> FlattenDictionary(
            Dictionary<AssetToValidate, RecursiveAssetValidation> resultDict)
        {
            var result = new List<InvalidAssetsEntry>();
            foreach (var invalidAsset in resultDict.Keys)
            {
                InvalidAssetsEntry entry =
                    new InvalidAssetsEntry(
                        invalidAsset.GetAssetNameSafe(),
                        invalidAsset.GetTopParentBasePath(),
                        "Unknown type");

                RecursiveAssetValidation errors = resultDict[invalidAsset];
                RecursiveFlatten(entry, errors, result);
            }

            return result;
        }

        private static void RecursiveFlatten(
            InvalidAssetsEntry entry,
            RecursiveAssetValidation errors,
            List<InvalidAssetsEntry> result)
        {
            foreach (var kvp in errors.InvalidFields)
            {
                var fieldName = kvp.Key.Name;
                var invalidField = kvp.Value;

                foreach (var invalidAttribute in invalidField.InvalidAttributes)
                {
                    AssetError error = new AssetError(invalidAttribute.GetAttributeType(), fieldName);
                    if (!result.Contains(entry))
                    {
                        entry.StringifiedType = invalidField.StringifiedType;
                        result.Add(entry);
                    }
                    entry.Errors.Add(error);
                }

                if (invalidField.IsFieldNotValue && invalidField.ChildsValidations.Count != 0)
                {
                    var oldEntry = entry;

                    foreach (RecursiveAssetValidation invalidAsset in invalidField.ChildsValidations)
                    {
                        entry = new InvalidAssetsEntry(
                            invalidAsset.AssetName,
                            oldEntry.AssetPath,
                            invalidField.StringifiedType);

                        RecursiveFlatten(entry, invalidAsset, result);
                    }
                }
            }

            foreach (var child in errors.InvalidChildren)
            {
                entry = new InvalidAssetsEntry(
                            child.AssetName,
                            entry.AssetPath,
                            "Transform");

                RecursiveFlatten(entry, child, result);
            }
        }

        public static AttributeValidationResult NormalAttributeValidationResult(
            Dictionary<AssetToValidate, RecursiveAssetValidation> dict)
        {
            var resultKind = dict.Keys.Count > 0 ? ResultKind.Failure : ResultKind.Success;
            var result = new AttributeValidationResult
            {
                Errors = FlattenDictionary(dict),
                m_invalidAssets = dict,
                ValidationResultKind = resultKind,
                InternalError = null,
            };

            return result;
        }

        public static AttributeValidationResult InternalErrorAttributeValidationResult(Exception e)
        {
            var result = new AttributeValidationResult
            {
                Errors = new List<InvalidAssetsEntry>(),
                m_invalidAssets = new Dictionary<AssetToValidate, RecursiveAssetValidation>(),
                ValidationResultKind = ResultKind.InternalError,
                InternalError = e,
            };

            return result;
        }

        public string ToJsonResult()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            jsonSerializerSettings.Formatting = Formatting.Indented;
            var text = JsonConvert.SerializeObject(this, jsonSerializerSettings);
            return text;
        }

        internal void AddRange(Dictionary<AssetToValidate, RecursiveAssetValidation> dict)
        {
            foreach (var kvp in dict)
            {
                m_invalidAssets.Add(kvp.Key, kvp.Value);
            }
        }
    }

    public class InvalidAssetsEntry
    {
        public InvalidAssetsEntry(string name, string path, string assetType)
        {
            Name = name;
            AssetPath = path;
            StringifiedType = assetType;
        }

        public string Name { get; private set; }

        public string AssetPath { get; private set; }

        public string StringifiedType { get; set; }

        public List<AssetError> Errors { get; private set; } = new List<AssetError>();

        internal string GetName()
        {
            return Name;
        }

        internal string GetAssetPath()
        {
            return AssetPath;
        }

        public override string ToString()
        {
            return $"{AssetPath}, {Name}, {StringifiedType}";
        }
    }

    public class InvalidAttributeDescription
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }

    public class AssetError
    {
        public AssetError(string attributeName, string fieldName)
        {
            AttributeErrorName = attributeName;
            FieldName = fieldName;
        }

        public string AttributeErrorName { get; private set; }

        public string FieldName { get; private set; }

        public override string ToString()
        {
            return $"AssetError: {FieldName}, {AttributeErrorName}";
        }
    }
}