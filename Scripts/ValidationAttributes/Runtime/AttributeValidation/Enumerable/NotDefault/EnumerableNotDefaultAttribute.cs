using System;

namespace AttributeValidation
{
    /// <summary>
    /// Attribute that causes the build to fail if any of the element of an enumerable is null
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumerableNotDefaultAttribute : EnumerableValidatableAttribute
    {
        protected override BaseValidator GetValidator()
        {
            return new NotDefaultValidator();
        }
    }
}