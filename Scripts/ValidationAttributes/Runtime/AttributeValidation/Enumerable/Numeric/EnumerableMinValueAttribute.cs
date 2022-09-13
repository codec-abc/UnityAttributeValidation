using System;

namespace AttributeValidation
{
    /// <summary>
    /// Attribute that causes the build to fail if any of the element of an enumerable is less than the min value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumerableMinValueAttribute : EnumerableValidatableAttribute
    {
        private readonly float m_minValue;

        public EnumerableMinValueAttribute(float min)
        {
            m_minValue = min;
        }

        protected override BaseValidator GetValidator()
        {
            return new MinValueValidator(m_minValue);
        }
    }
}