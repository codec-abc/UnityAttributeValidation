using System;
using static AttributeValidation.CustomFunctionValidator;

/// <summary>
/// see https://stackoverflow.com/questions/7705386/is-it-possible-to-have-a-delegate-as-attribute-parameter
/// </summary>

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CustomFunctionValidatorAttribute : SingleElementValidatableAttribute
    {
        private readonly ValidateDelegate m_customValidationFunction;

        public CustomFunctionValidatorAttribute(Type delegateType, string delegateName)
        {
            m_customValidationFunction =
                (ValidateDelegate)Delegate.CreateDelegate(typeof(ValidateDelegate), delegateType, delegateName);
        }

        protected override BaseValidator GetValidator()
        {
            return new CustomFunctionValidator(m_customValidationFunction);
        }
    }
}