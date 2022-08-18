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

        public RecursiveAssetValidation(
            string assetName,
            bool isValid,
            Dictionary<FieldInfo, RecursiveFieldValidation> invalidFields)
        {
            AssetName = assetName;
            IsValid = isValid;
            InvalidFields = invalidFields;
        }
    }
}