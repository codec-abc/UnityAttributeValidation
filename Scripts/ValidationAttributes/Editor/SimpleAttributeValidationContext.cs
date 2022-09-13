using System.Collections.Generic;
using System.Reflection;
using DUDE.Core.Attributes;
using UnityEngine;

namespace AttributeValidation
{
    internal class SimpleAttributeValidationContext : IValidationContext
    {
        private readonly Dictionary<System.Type, BaseValidator> m_extendedAttributeValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
                // { typeof(RangeAttribute), new RangeValidator() },
                { typeof(DUDE.Core.Attributes.ObjectDatabaseAttribute), new ObjectDatabaseValidator() },
            };

        private readonly Dictionary<System.Type, BaseValidator> m_extendedFieldValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
                { typeof(ObjectReference), new ObjectReferenceValidator() },
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
    }
}