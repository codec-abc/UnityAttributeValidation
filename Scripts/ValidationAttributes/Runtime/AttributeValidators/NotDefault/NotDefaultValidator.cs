using System;
using System.Reflection;

namespace AttributeValidation
{
    public class NotDefaultValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            var type = fieldInfo.FieldType;
            var defaultValue = GetDefault(type);
            return !attributeFieldObj.Equals(defaultValue);
        }

        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}