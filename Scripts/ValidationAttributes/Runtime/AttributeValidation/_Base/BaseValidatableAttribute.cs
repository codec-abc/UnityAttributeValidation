using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    public abstract class BaseValidatableAttribute : PropertyAttribute
    {
        protected BaseValidator m_validator;

        protected BaseValidatableAttribute()
        {
            m_validator = GetValidator();
        }

        public abstract bool Validate(object obj, object parentObj, FieldInfo fieldInfo, IValidationContext validationContext);

        protected abstract BaseValidator GetValidator();

        public virtual string GetErrorMessage(string invalidFieldName)
        {
            return invalidFieldName + " field is not valid";
        }
    }
}