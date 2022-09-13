using System;

namespace AttributeValidation
{
    /// <summary>
    /// Attribute that causes the build to fail if its target is greater than the max value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MaxValueAttribute : SingleElementValidatableAttribute
    {
        private readonly float m_maxValue;

        public MaxValueAttribute(float max)
        {
            m_maxValue = max;
        }

        protected override BaseValidator GetValidator()
        {
            return new MaxValueValidator(m_maxValue);
        }
    }
}