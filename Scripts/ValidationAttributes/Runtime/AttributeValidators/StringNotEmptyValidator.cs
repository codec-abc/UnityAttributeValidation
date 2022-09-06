using System.Reflection;

namespace AttributeValidation
{
    public class StringNotEmptyValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (!(attributeFieldObj is string str))
            {
                var msg = $"Cannot apply {typeof(StringNotEmptyValidator)} to a null variable or variable which is not a string";
                throw new System.Exception(msg);
            }

            return !string.IsNullOrEmpty(str);
        }
    }
}
