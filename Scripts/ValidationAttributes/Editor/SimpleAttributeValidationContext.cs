using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    internal class SimpleAttributeValidationContext : IValidationContext
    {
        private readonly Dictionary<System.Type, BaseValidator> m_extendedValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
                { typeof(RangeAttribute), new RangeValidator() },
            };

        public bool ShouldIgnoreObj(object obj, object parentObj, FieldInfo fieldInfo)
        {
            return false;
        }

        public bool ShouldIgnoreEnumerableObj(object enuObj, object obj, object parentObj, FieldInfo fieldInfo)
        {
            return false;
        }

        public void AddAssetsToValidate(IAssetsToValidateCollection attributeValidationScript)
        {
        }

        public IValidationContext.CustomLogicResult CustomLogicForAsset(
            string path,
            Object loadedObj,
            IAssetsToValidateCollection assetsToValidateCollection)
        {
            return IValidationContext.CustomLogicResult.Process;
        }

        public IReadOnlyDictionary<System.Type, BaseValidator> GetExtendedAttributeValidator()
        {
            return m_extendedValidators;
        }
    }
}