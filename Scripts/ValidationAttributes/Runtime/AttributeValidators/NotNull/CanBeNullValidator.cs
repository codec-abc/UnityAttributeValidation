using System.Reflection;

namespace AttributeValidation
{
    public class CanBeNullValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj != null && !attributeFieldObj.GetType().IsValueType)
            {
                return true;
            }
            else if (attributeFieldObj == null)
            {
                return true;
            }

            var msg = $"Cannot apply {typeof(NotNullValidator)} to variables of type {attributeFieldObj.GetType()} for field {fieldInfo}";
            throw new System.Exception(msg);
        }
    }
}