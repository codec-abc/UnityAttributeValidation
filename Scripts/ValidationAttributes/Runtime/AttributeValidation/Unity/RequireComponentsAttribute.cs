using System;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class RequireComponentsAttribute : SingleElementValidatableAttribute
    {
        private readonly RequireComponentValidator.HierarchyPosition m_hierarchyPosition;

        private readonly RequireComponentValidator.SearchOption m_searchOption =
            RequireComponentValidator.SearchOption.ExcludeInactive;

        private readonly int m_numMinToFind = 1;

        private readonly Type m_typeToFind;

        public RequireComponentsAttribute(
            Type typeToFind,
            RequireComponentValidator.HierarchyPosition hierarchyPosition)
        {
            m_typeToFind = typeToFind;
            m_hierarchyPosition = hierarchyPosition;
        }

        public RequireComponentsAttribute(
            Type typeToFind,
            RequireComponentValidator.HierarchyPosition hierarchyPosition,
            int numMinToFind) : this(typeToFind, hierarchyPosition)
        {
            m_numMinToFind = numMinToFind;
        }

        public RequireComponentsAttribute(
            Type typeToFind,
            RequireComponentValidator.HierarchyPosition hierarchyPosition,
            RequireComponentValidator.SearchOption searchOption) : this(typeToFind, hierarchyPosition)
        {
            m_searchOption = searchOption;
        }

        public RequireComponentsAttribute(
            Type typeToFind,
            RequireComponentValidator.HierarchyPosition hierarchyPosition,
            RequireComponentValidator.SearchOption searchOption,
            int numMinToFind) : this(typeToFind, hierarchyPosition, searchOption)
        {
            m_numMinToFind = numMinToFind;
        }

        protected override BaseValidator GetValidator()
        {
            return new RequireComponentValidator(
                m_hierarchyPosition,
                m_searchOption,
                m_numMinToFind,
                m_typeToFind);
        }
    }
}