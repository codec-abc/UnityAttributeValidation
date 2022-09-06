using System;
using System.Collections.Generic;
using System.Reflection;

namespace AttributeValidation
{
    public interface IValidationContext
    {
        bool ShouldIgnoreObj(object obj, object parentObj, FieldInfo fieldInfo);

        bool ShouldIgnoreEnumerableObj(object enuObj, object obj, object parentObj, FieldInfo fieldInfo);

        void AddAssetsToValidate(IAssetsToValidateCollection assetsToValidateCollection);

        public enum CustomLogicResult
        {
            Ignore,
            Process,
        }

        CustomLogicResult CustomLogicForAsset(
            string path,
            UnityEngine.Object loadedObj,
            IAssetsToValidateCollection assetsToValidateCollection);

        public IReadOnlyDictionary<Type, BaseValidator> GetExtendedAttributeValidator();
    }

    public interface IAssetsToValidateCollection
    {
        void AddIfNotAlreadyInAnalyzeList(AssetToValidate assetToValidate);
    }
}