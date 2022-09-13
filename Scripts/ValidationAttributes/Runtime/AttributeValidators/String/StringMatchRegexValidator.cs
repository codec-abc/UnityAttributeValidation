using System.Reflection;
using System.Text.RegularExpressions;

namespace AttributeValidation
{
    public class StringMatchRegexValidator : BaseValidator
    {
        private readonly string m_pattern;

        public StringMatchRegexValidator(string pattern)
        {
            m_pattern = pattern;
        }

        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (!(attributeFieldObj is string str))
            {
                var msg = $"Cannot apply {typeof(StringMatchRegexValidator)} to a null variable or variable which is not a string for field {fieldInfo}";
                throw new System.Exception(msg);
            }

            if (m_pattern == null)
            {
                throw new System.Exception("Regex pattern of validator should not be null.");
            }

            return Regex.IsMatch(str, m_pattern);
        }
    }
}