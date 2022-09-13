using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumerableHierarchyValidationAttribute : EnumerableValidatableAttribute
    {
        private readonly HierarchyValidator.HierarchyPosition m_allowedHierarchy;

        public EnumerableHierarchyValidationAttribute(HierarchyValidator.HierarchyPosition allowedHierarchy)
        {
            m_allowedHierarchy = allowedHierarchy;
        }

        protected override BaseValidator GetValidator()
        {
            return new HierarchyValidator(m_allowedHierarchy);
        }
    }
}