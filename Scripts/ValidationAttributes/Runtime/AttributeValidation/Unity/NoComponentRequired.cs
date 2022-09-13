using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NoComponentRequired : SingleElementValidatableAttribute
    {
        protected override BaseValidator GetValidator()
        {
            return new AlwaysGoodValidator();
        }
    }
}