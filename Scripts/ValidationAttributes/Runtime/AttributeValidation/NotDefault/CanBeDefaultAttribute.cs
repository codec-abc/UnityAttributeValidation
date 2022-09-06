using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CanBeDefaultAttribute : SingleElementValidatableAttribute
    {
        protected override BaseValidator GetValidator()
        {
            return new AlwaysGoodValidator();
        }
    }
}