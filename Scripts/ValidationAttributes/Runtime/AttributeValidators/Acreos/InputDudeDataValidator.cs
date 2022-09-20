using System.Reflection;

namespace AttributeValidation
{
    public class InputDudeDataValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            var data = (DUDE.InputDudeData)attributeFieldObj;
            var allValid = true;

            allValid &= AddressableValidator.IsAddressValidWeak(data.InversedTutorialVideoAddress);
            allValid &= AddressableValidator.IsAddressValidWeak(data.NormalTutorialVideoAddress);
            allValid &= AddressableValidator.IsAddressValidWeak(data.TutorialImageAddress);

            return allValid;
        }
    }
}
