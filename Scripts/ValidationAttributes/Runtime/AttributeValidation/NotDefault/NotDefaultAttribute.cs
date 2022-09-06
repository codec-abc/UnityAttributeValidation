using System;

namespace AttributeValidation
{
    /// <summary>
    /// Attribute that causes the build to fail if its target has the default value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NotDefaultAttribute : SingleElementValidatableAttribute
    {
        protected override BaseValidator GetValidator()
        {
            return new NotDefaultValidator();
        }
    }
}