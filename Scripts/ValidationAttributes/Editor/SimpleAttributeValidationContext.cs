using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    internal class SimpleAttributeValidationContext : IValidationContext
    {
        private readonly Dictionary<System.Type, BaseValidator> m_extendedAttributeValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
                // { typeof(RangeAttribute), new RangeValidator() },
            };

        private readonly Dictionary<System.Type, BaseValidator> m_extendedFieldValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
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