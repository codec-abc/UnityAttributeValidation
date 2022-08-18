using System;

namespace AttributeValidation
{
    /// <summary>
    /// Attribute that causes the build to fail if its target is null
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NotNullAttribute : SingleElementValidatableAttribute
    {
        protected override BaseValidator GetValidator()
        {
            return new NotNullValidator();
        }
    }
}