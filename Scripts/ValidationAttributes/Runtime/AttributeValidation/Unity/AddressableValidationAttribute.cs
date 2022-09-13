using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AddressableValidationAttribute : SingleElementValidatableAttribute
    {
        protected override BaseValidator GetValidator()
        {
            return new AddressableValidator();
        }
    }
}