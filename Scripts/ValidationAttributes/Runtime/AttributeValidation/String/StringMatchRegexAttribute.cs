using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StringMatchRegexAttribute : SingleElementValidatableAttribute
    {
        private readonly string m_pattern;

        public StringMatchRegexAttribute(string pattern)
        {
            m_pattern = pattern;
        }

        protected override BaseValidator GetValidator()
        {
            return new StringMatchRegexValidator(m_pattern);
        }
    }
}