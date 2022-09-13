using System.Reflection;

/// <summary>
/// see https://stackoverflow.com/questions/7705386/is-it-possible-to-have-a-delegate-as-attribute-parameter
/// </summary>

namespace AttributeValidation
{
    public class CustomFunctionValidator : BaseValidator
    {
        public delegate bool ValidateDelegate(object arg1, object arg2, FieldInfo arg3);

        private readonly ValidateDelegate m_customValidationFunction;

        public CustomFunctionValidator(ValidateDelegate customValidationFunction)
        {
            m_customValidationFunction = customValidationFunction;
        }

        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            return m_customValidationFunction(attributeFieldObj, ownerObj, fieldInfo);
        }
    }
}