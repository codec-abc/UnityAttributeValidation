using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StringNotWhiteSpaceAttribute : SingleElementValidatableAttribute
    {
        protected override BaseValidator GetValidator()
        {
            return new StringNotWhiteSpaceValidator();
        }
    }
}
