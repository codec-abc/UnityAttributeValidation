using System.Collections.Generic;
using System.Reflection;

namespace AttributeValidation
{
    /// <summary>
    /// Data structure which store, for an object, whether all its fields are valid and all the invalid fields
    /// </summary>
    public class RecursiveAssetValidation
    {
        public readonly string AssetName;
        public readonly bool IsValid;

        public readonly Dictionary<FieldInfo, RecursiveFieldValidation> InvalidFields;
        public readonly List<RecursiveAssetValidation> InvalidChildren;

        public RecursiveAssetValidation(
            string assetName,
            bool isValid,
            Dictionary<FieldInfo, RecursiveFieldValidation> invalidFields,
            List<RecursiveAssetValidation> invalidChidren)
        {
            AssetName = assetName;
            IsValid = isValid;
            InvalidFields = invalidFields;
            InvalidChildren = invalidChidren;
        }

        public override string ToString()
        {
            var msg = $"{AssetName}, IsValid: {IsValid}: ";

            foreach (var kvp in InvalidFields)
            {
                msg += $"{kvp.Key.Name}";
            }

            foreach (var child in InvalidChildren)
            {
                msg += $"{child}\n";
            }

            return msg;
        }
    }
}