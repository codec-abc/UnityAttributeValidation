using System.Collections.Generic;

namespace AttributeValidation
{
    /// <summary>
    /// A data structure that stores, for a field, whether it is valid, all its invalid attributes and
    /// whether it is not a value field, and a list containing as many RecursiveAssetValidations
    /// as there are objects referenced by the field.
    /// </summary>
    public class RecursiveFieldValidation
    {
        public readonly string StringifiedType;

        public readonly bool IsValid;
        public readonly List<InvalidAttribute> InvalidAttributes;

        // public readonly List<BaseValidatableAttribute> InvalidAttributes;
        public class InvalidAttribute
        {
            private readonly string m_attributeName;

            public InvalidAttribute(string attributeName)
            {
                m_attributeName = attributeName;
            }

            internal string GetAttributeType()
            {
                return m_attributeName;
            }
        }

        public readonly bool IsFieldNotValue;
        public readonly List<RecursiveAssetValidation> ChildsValidations;

        public RecursiveFieldValidation(
            string stringifiedType,
            bool isValid,
            List<InvalidAttribute> invalidAttributes)
        {
            StringifiedType = stringifiedType;
            IsValid = isValid;
            InvalidAttributes = invalidAttributes;
            IsFieldNotValue = false;
        }

        public RecursiveFieldValidation(
            string stringifiedType,
            bool isValid,
            List<InvalidAttribute> invalidAttributes,
            List<RecursiveAssetValidation> childValidation) : this(stringifiedType, isValid, invalidAttributes)
        {
            IsFieldNotValue = true;
            ChildsValidations = childValidation;
        }
    }
}