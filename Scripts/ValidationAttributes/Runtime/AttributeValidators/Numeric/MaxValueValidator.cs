using System.Reflection;

namespace AttributeValidation
{
    public class MaxValueValidator : BaseValidator
    {
        private readonly float m_maxValue;

        public MaxValueValidator(float maxValue)
        {
            m_maxValue = maxValue;
        }

        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (AttributeValidationUtils.TryCastToFloat(attributeFieldObj, out float objValue))
            {
                return m_maxValue >= objValue;
            }
            else
            {
                throw new System.Exception($"[{attributeFieldObj}] cannot compare {attributeFieldObj.GetType()} and {typeof(float)}  for field {fieldInfo}");
            }
        }
    }
}