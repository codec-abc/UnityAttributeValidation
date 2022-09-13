using System;
using System.Collections.Generic;
using System.Linq;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumerablePathValidationAttribute : EnumerableValidatableAttribute
    {
        private readonly string m_startingPath = "";
        private readonly PathValidator.PathForObjectKind m_targetedObjectByPath = PathValidator.PathForObjectKind.Any;
        private readonly PathValidator.PathKind m_pathKind;
        private readonly List<string> m_allowedExtensions = new List<string> { "*" };

        public EnumerablePathValidationAttribute(PathValidator.PathKind pathKind)
        {
            m_pathKind = pathKind;
        }

        public EnumerablePathValidationAttribute(
            PathValidator.PathKind pathKind,
            PathValidator.PathForObjectKind targetedObjectByPath)
        {
            m_pathKind = pathKind;
            m_targetedObjectByPath = targetedObjectByPath;
        }

        public EnumerablePathValidationAttribute(
            PathValidator.PathKind pathKind,
            PathValidator.PathForObjectKind targetedObjectByPath,
            string startingPath)
        {
            m_pathKind = pathKind;
            m_targetedObjectByPath = targetedObjectByPath;
            m_startingPath = startingPath;
        }

        public EnumerablePathValidationAttribute(
            PathValidator.PathKind pathKind,
            string startingPath,
            params string[] allowedExtensions)
        {
            m_startingPath = startingPath;
            m_pathKind = pathKind;
            if (allowedExtensions == null || allowedExtensions.Length == 0)
            {
                m_allowedExtensions = new List<string>() { "*" };
            }
            else
            {
                m_allowedExtensions = allowedExtensions.ToList();
            }
            m_targetedObjectByPath = PathValidator.PathForObjectKind.File;
        }

        protected override BaseValidator GetValidator()
        {
            return new PathValidator(
                m_startingPath,
                m_targetedObjectByPath,
                m_pathKind,
                m_allowedExtensions);
        }
    }
}