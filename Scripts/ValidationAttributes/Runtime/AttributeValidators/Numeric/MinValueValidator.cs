using System.Reflection;

namespace AttributeValidation
{
    public class MinValueValidator : BaseValidator
    {
        private readonly float m_minValue;

        public MinValueValidator(float minValue)
        {
            m_minValue = minValue;
        }

        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (AttributeValidationUtils.TryCastToFloat(attributeFieldObj, out float objValue))
            {
                return objValue >= m_minValue;
            }
            else
            {
                throw new System.Exception($"[{attributeFieldObj}] cannot compare {attributeFieldObj.GetType()} and {typeof(float)}  for field {fieldInfo}");
            }
        }
    }
}