using System.Reflection;

namespace AttributeValidation
{
    public class DUDEHardwareDataValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            var data = (DUDE.HardwareData)attributeFieldObj;
            var allValid = true;

            allValid &= AddressableValidator.IsAddressValidWeak(data.HardwareImageAddress);

            return allValid;
        }
    }
}
