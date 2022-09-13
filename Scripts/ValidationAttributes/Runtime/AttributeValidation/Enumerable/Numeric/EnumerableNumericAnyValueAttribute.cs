using System;

namespace AttributeValidation
{
    /// <summary>
    /// Attribute that causes the build to fail if any of the element of an enumerable is less than the min value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumerableNumericAnyValueAttribute : EnumerableValidatableAttribute
    {
        protected override BaseValidator GetValidator()
        {
            return new AlwaysGoodValidator();
        }
    }
}
