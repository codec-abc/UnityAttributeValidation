using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumerableStringMatchRegexAttribute : EnumerableValidatableAttribute
    {
        private readonly string m_pattern;

        public EnumerableStringMatchRegexAttribute(string pattern)
        {
            m_pattern = pattern;
        }

        protected override BaseValidator GetValidator()
        {
            return new StringMatchRegexValidator(m_pattern);
        }
    }
}