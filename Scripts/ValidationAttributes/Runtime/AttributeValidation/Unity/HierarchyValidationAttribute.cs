using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class HierarchyValidationAttribute : SingleElementValidatableAttribute
    {
        private readonly HierarchyValidator.HierarchyPosition m_allowedHierarchy;

        public HierarchyValidationAttribute(HierarchyValidator.HierarchyPosition allowedHierarchy)
        {
            m_allowedHierarchy = allowedHierarchy;
        }

        protected override BaseValidator GetValidator()
        {
            return new HierarchyValidator(m_allowedHierarchy);
        }
    }
}