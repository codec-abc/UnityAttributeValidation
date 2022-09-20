using System.Reflection;

namespace AttributeValidation
{
    public class DUDECoreInputsInputValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            var data = (DUDE.Core.Inputs.Input)attributeFieldObj;
            var allValid = true;

            allValid &= AddressableValidator.IsAddressValidWeak(data.InversedTutorialVideoAddress);
            allValid &= AddressableValidator.IsAddressValidWeak(data.TutorialImageAddress);
            allValid &= AddressableValidator.IsAddressValidWeak(data.TutorialVideoAddress);

            return allValid;
        }
    }
}
