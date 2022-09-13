using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumerableNoHierarchyValidationAttribute : EnumerableValidatableAttribute
    {
        protected override BaseValidator GetValidator()
        {
            return new AlwaysGoodValidator();
        }
    }
}