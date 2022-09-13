using System.Reflection;

namespace AttributeValidation
{
    public class AlwaysGoodValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            return true;
        }
    }
}