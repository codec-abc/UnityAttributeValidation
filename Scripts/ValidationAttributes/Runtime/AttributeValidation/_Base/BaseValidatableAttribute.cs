using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    public abstract class BaseValidatableAttribute : PropertyAttribute
    {
        public abstract bool Validate(
            object obj,
            object parentObj,
            FieldInfo fieldInfo,
            IValidationContext validationContext);

        protected abstract BaseValidator GetValidator();

        public virtual string GetErrorMessage(string invalidFieldName)
        {
            return invalidFieldName + " field is not valid";
        }
    }
}