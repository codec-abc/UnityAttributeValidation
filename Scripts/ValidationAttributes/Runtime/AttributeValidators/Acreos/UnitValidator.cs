using System.Reflection;

namespace AttributeValidation
{
    public class UnitValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            var value = (DUDE.Core.Unit)attributeFieldObj;

            return
                !string.IsNullOrWhiteSpace(value.UnitKey); // &&

            // !string.IsNullOrWhiteSpace(value.Format);
        }
    }
}
