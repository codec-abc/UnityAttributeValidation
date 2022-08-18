using System.Reflection;

namespace AttributeValidation
{
    public abstract class SingleElementValidatableAttribute : BaseValidatableAttribute
    {
        public override bool Validate(
            object obj,
            object parentObj,
            FieldInfo fieldInfo,
            IValidationContext validationContext)
        {
            if (validationContext.ShouldIgnoreObj(obj, parentObj, fieldInfo))
            {
                return true;
            }
            return m_validator.Validate(obj, parentObj, fieldInfo);
        }
    }
}