using System.Reflection;

namespace AttributeValidation
{
    public class StringNotWhiteSpaceValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (!(attributeFieldObj is string str))
            {
                var msg = $"Cannot apply {typeof(StringNotWhiteSpaceValidator)} to a null variable or variable which is not a string  for field {fieldInfo}";
                throw new System.Exception(msg);
            }

            return !string.IsNullOrWhiteSpace(str);
        }
    }
}