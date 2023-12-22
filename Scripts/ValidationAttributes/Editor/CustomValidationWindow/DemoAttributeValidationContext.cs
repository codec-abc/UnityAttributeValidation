using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AttributeValidation
{
    internal class DemoAttributeValidationContext : IValidationContext
    {
        private readonly Dictionary<System.Type, BaseValidator> m_extendedAttributeValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
                // Too many errors
                // { typeof(RangeAttribute), new RangeValidator() },,
            };

        private readonly Dictionary<System.Type, BaseValidator> m_extendedFieldValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
            };

        public bool ShouldIgnoreObj(object obj, object parentObj, FieldInfo fieldInfo)
        {
            if (ObjectValidationFilters.IsUnityObjImageParameter(parentObj))
            {
                return true;
            }

            return false;
        }

        public IValidationContext.CustomLogicResult CustomLogicForAsset(
            string path,
            Object loadedObj,
            IAssetsToValidateCollection assetsToValidateCollection)
        {
            var isTemplate = IsAssetTemplate(path, loadedObj);

            if (isTemplate)
            {
                return IValidationContext.CustomLogicResult.Ignore;
            }

            return IValidationContext.CustomLogicResult.Process;
        }

        public void AddAssetsToValidate(IAssetsToValidateCollection assetsToValidateCollection)
        {
        }

        public bool ShouldIgnoreEnumerableObj(object enuObj, object obj, object parentObj, FieldInfo fieldInfo)
        {
            if (ObjectValidationFilters.IsUnityObjImageParameter(parentObj))
            {
                return true;
            }

            return false;
        }

        private static bool IsAssetTemplate(string path, Object loadedObj)
        {
            var labels = AssetDatabase.GetLabels(loadedObj);
            if (labels != null)
            {
                if (labels.Any(l => l == "IgnoreAttributeValidation"))
                {
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyDictionary<System.Type, BaseValidator> GetExtendedAttributeValidators()
        {
            return m_extendedAttributeValidators;
        }

        public IReadOnlyDictionary<System.Type, BaseValidator> GetExtendedFieldValidators()
        {
            return m_extendedFieldValidators;
        }

        public bool ShouldIgnoreType(System.Type type)
        {
            return false;
        }
    }
}