using System;

namespace AttributeValidation
{
    /// <summary>
    /// Attribute that causes the build to fail if any of the element of an enumerable is greater than the max value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumerableMaxValueAttribute : EnumerableValidatableAttribute
    {
        private readonly float m_maxValue;

        public EnumerableMaxValueAttribute(float max)
        {
            m_maxValue = max;
        }

        protected override BaseValidator GetValidator()
        {
            return new MaxValueValidator(m_maxValue);
        }
    }
}