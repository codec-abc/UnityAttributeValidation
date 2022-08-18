using System.Reflection;
using UnityEngine;

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
            Object loadedObj,
            IAssetsToValidateCollection assetsToValidateCollection);
    }

    public interface IAssetsToValidateCollection
    {
        void AddIfNotAlreadyInAnalyzeList(AssetToValidate assetToValidate);
    }
}