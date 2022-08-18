using System;

namespace AttributeValidation
{
    /// <summary>
    /// Attribute that causes the build to fail if its target is less than the min value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MinValueAttribute : SingleElementValidatableAttribute
    {
        private readonly float m_minValue;

        public MinValueAttribute(float min)
        {
            m_minValue = min;
        }

        protected override BaseValidator GetValidator()
        {
            return new MinValueValidator(m_minValue);
        }
    }
}