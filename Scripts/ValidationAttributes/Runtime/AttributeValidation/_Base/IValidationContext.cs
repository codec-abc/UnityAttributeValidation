using System;
using System.Collections.Generic;
using System.Reflection;

namespace AttributeValidation
{
    public interface IValidationContext
    {
        bool ShouldIgnoreObj(object obj, object parentObj, FieldInfo fieldInfo);

        bool ShouldIgnoreEnumerableObj(object enuObj, object obj, object parentObj, FieldInfo fieldInfo);

        bool ShouldIgnoreType(Type type);

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

        IReadOnlyDictionary<Type, BaseValidator> GetExtendedAttributeValidators();

        IReadOnlyDictionary<Type, BaseValidator> GetExtendedFieldValidators();
    }

    public interface IAssetsToValidateCollection
    {
        void AddIfNotAlreadyInAnalyzeList(AssetToValidate assetToValidate);
    }
}