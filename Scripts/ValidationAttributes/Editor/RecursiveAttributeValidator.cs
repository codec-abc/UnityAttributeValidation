using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace AttributeValidation
{
    public class RecursiveAttributeValidator
    {
        public class MemorizedObjectsValidation
        {
            private readonly List<ObjectWithRecursiveValidation> m_memorizedObjectValidationAsList =
                new List<ObjectWithRecursiveValidation>();

            private readonly Dictionary<object, RecursiveAssetValidation> m_memorizedObjectValidationAsDict =
                new Dictionary<object, RecursiveAssetValidation>();

            public class ObjectWithRecursiveValidation
            {
                public object Obj;
                public RecursiveAssetValidation Validation;

                public ObjectWithRecursiveValidation(object asset, RecursiveAssetValidation p)
                {
                    Obj = asset;
                    Validation = p;
                }
            }

            internal (bool, RecursiveAssetValidation) GetEntry(object asset)
            {
                try
                {
                    if (m_memorizedObjectValidationAsDict.TryGetValue(asset, out var returned))
                    {
                        return (true, returned);
                    }

                    return (false, null);
                }
                catch (Exception)
                {
                    var entry =
                        m_memorizedObjectValidationAsList
                        .FirstOrDefault(x => x.Obj == asset);

                    return (true, entry?.Validation);
                }
            }

            internal ObjectWithRecursiveValidation Add(object assetToValidate, RecursiveAssetValidation p)
            {
                m_memorizedObjectValidationAsDict.Add(assetToValidate, p);
                var returned = new ObjectWithRecursiveValidation(assetToValidate, p);
                m_memorizedObjectValidationAsList.Add(returned);
                return returned;
            }

            internal void SetValidation(
                ObjectWithRecursiveValidation entry,
                object assetToValidate,
                RecursiveAssetValidation objValidation)
            {
                entry.Validation = objValidation;
                try
                {
                    m_memorizedObjectValidationAsDict[assetToValidate] = objValidation;
                }
                catch (Exception)
                {
                }
            }
        }

        public static Dictionary<AssetToValidate, RecursiveAssetValidation> GetAllInvalidObjectsRecursively(
            AssetToValidate[] assetsToCheck,
            IValidationContext validationContext)
        {
            Dictionary<AssetToValidate, RecursiveAssetValidation> invalidObjects =
                new Dictionary<AssetToValidate, RecursiveAssetValidation>();

            MemorizedObjectsValidation memorizedObjectValidation = new MemorizedObjectsValidation();

            int currentAssetIndex = 0;

            foreach (AssetToValidate assetToValidate in assetsToCheck)
            {
                var name = assetToValidate.GetAssetNameSafe();

                if (EditorUtility.DisplayCancelableProgressBar(
                    "Checking assets",
                    name,
                    (float)currentAssetIndex / assetsToCheck.Length))
                {
                    break;
                }

                if (assetToValidate != null)
                {
                    if (!invalidObjects.ContainsKey(assetToValidate))
                    {
                        var (hasValidation, validation) = memorizedObjectValidation.GetEntry(assetToValidate.Asset);
                        if (!hasValidation)
                        {
                            var newerEntry = memorizedObjectValidation.Add(assetToValidate.Asset, null);
                            validation = GetRecursiveObjectValidation(assetToValidate, memorizedObjectValidation, validationContext);
                            memorizedObjectValidation.SetValidation(newerEntry, assetToValidate.Asset, validation);
                        }

                        if (validation != null)
                        {
                            if (!validation.IsValid)
                            {
                                invalidObjects.Add(assetToValidate, validation);
                            }
                        }
                    }
                }
                currentAssetIndex++;
            }

            return invalidObjects;
        }

        private static RecursiveAssetValidation GetRecursiveObjectValidation(
            AssetToValidate assetToValidate,
            MemorizedObjectsValidation memorizedObjects,
            IValidationContext validationContext)
        {
            bool isObjValid = true;

            FieldInfo[] fieldsInfos =
                assetToValidate.Asset.GetType()
                .GetFields(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

            var invalidFields = new Dictionary<FieldInfo, RecursiveFieldValidation>();

            foreach (FieldInfo field in fieldsInfos)
            {
                var attributes = field.CustomAttributes;

                var shouldBeAnalyzed = field.Attributes == FieldAttributes.Public;

                if (!shouldBeAnalyzed)
                {
                    foreach (var attribute in attributes)
                    {
                        if (attribute.AttributeType == typeof(UnityEngine.SerializeField))
                        {
                            shouldBeAnalyzed = true;
                        }
                    }
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.AttributeType == typeof(System.NonSerializedAttribute))
                    {
                        shouldBeAnalyzed = false;
                    }
                }

                if (!shouldBeAnalyzed)
                {
                    continue;
                }

                var fieldValidation = GetRecursiveFieldValidation(assetToValidate, field, memorizedObjects, validationContext);

                if (!fieldValidation.IsValid)
                {
                    invalidFields.Add(field, fieldValidation);
                    isObjValid = false;
                }
            }

            return new RecursiveAssetValidation(assetToValidate.GetAssetNameSafe(), isObjValid, invalidFields);
        }

        private static RecursiveFieldValidation GetRecursiveFieldValidation(
            AssetToValidate assetToValidate,
            FieldInfo fieldInfo,
            MemorizedObjectsValidation memorizedObjects,
            IValidationContext validationContext)
        {
            var assetTypeAsString = assetToValidate.Asset.GetType().ToString();
            bool isFieldValid = true;
            var invalidAttributes = new List<BaseValidatableAttribute>();

            foreach (var attribute in Attribute.GetCustomAttributes(fieldInfo, typeof(BaseValidatableAttribute)))
            {
                var validatableAttribute = (BaseValidatableAttribute)attribute;

                if (!validatableAttribute.Validate(
                    fieldInfo.GetValue(assetToValidate.Asset),
                    assetToValidate.Asset,
                    fieldInfo,
                    validationContext))
                {
                    invalidAttributes.Add(validatableAttribute);
                    isFieldValid = false;
                }
            }

            object fieldInfoValue = fieldInfo.GetValue(assetToValidate.Asset);

            // crash Unity if the field is not a value type
            // TODO: should open a bug for this
            if (!fieldInfo.FieldType.IsValueType)
            {
                var childValidation = new List<RecursiveAssetValidation>();

                AddObjectValidation(
                    new AssetToValidate(fieldInfoValue, assetToValidate),
                    memorizedObjects,
                    childValidation,
                    validationContext);

                bool areAllChildsValid =
                    !childValidation
                    .Any((RecursiveAssetValidation objVal) => !objVal.IsValid);

                return new RecursiveFieldValidation(
                    assetTypeAsString,
                    isFieldValid && areAllChildsValid,
                    invalidAttributes,
                    childValidation);
            }
            else
            {
                return new RecursiveFieldValidation(assetTypeAsString, isFieldValid, invalidAttributes);
            }
        }

        private static void AddObjectValidation(
            AssetToValidate assetToValidate,
            MemorizedObjectsValidation memorizedObjects,
            List<RecursiveAssetValidation> listToAppend,
            IValidationContext validationContext)
        {
            var (hasValidation, validation) = memorizedObjects.GetEntry(assetToValidate.Asset);
            if (!hasValidation)
            {
                var newerEntry = memorizedObjects.Add(assetToValidate.Asset, null);
                validation = GetRecursiveObjectValidation(assetToValidate, memorizedObjects, validationContext);
                memorizedObjects.SetValidation(newerEntry, assetToValidate, validation);
            }

            if (validation != null && !validation.IsValid)
            {
                listToAppend.Add(validation);
            }
        }

        private static bool IsUnityObjAlive(UnityEngine.Object obj)
        {
#pragma warning disable RS0030
            return obj;
#pragma warning restore
        }
    }
}