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
            var validator = GetValidator();

            if (validationContext.ShouldIgnoreObj(obj, parentObj, fieldInfo))
            {
                return true;
            }
            return validator.Validate(obj, parentObj, fieldInfo);
        }
    }
}