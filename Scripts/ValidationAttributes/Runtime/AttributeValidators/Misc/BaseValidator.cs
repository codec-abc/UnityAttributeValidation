using System.Reflection;

namespace AttributeValidation
{
    public abstract class BaseValidator
    {
        public abstract bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo);
    }
}