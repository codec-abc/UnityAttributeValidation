using System.Reflection;

namespace AttributeValidation
{
    public class DUDECoreInputsAxisValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            var data = (DUDE.Core.Inputs.Axis)attributeFieldObj;
            var allValid = true;

            allValid &= AddressableValidator.IsAddressValidWeak(data.InversedTutorialVideoAddress);
            allValid &= AddressableValidator.IsAddressValidWeak(data.TutorialImageAddress);
            allValid &= AddressableValidator.IsAddressValidWeak(data.TutorialVideoAddress);
            allValid &= AddressableValidator.IsAddressValidWeak(data.CalibrationImage);
            allValid &= AddressableValidator.IsAddressValidWeak(data.CalibrationStaticImage);

            return allValid;
        }
    }
}
