using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    public class RequireComponentValidator : BaseValidator
    {
        public enum HierarchyPosition
        {
            Self,
            ParentsAndSelf,
            ChildsAndSelf,
        }

        public enum SearchOption
        {
            IncludeInactive,
            ExcludeInactive,
        }

        private readonly HierarchyPosition m_componentSearchPath;
        private readonly SearchOption m_componentSearchOption;
        private readonly int m_numberMinToFind;
        private readonly Type m_typeToFind;

        public RequireComponentValidator(
            HierarchyPosition componentSearchPath,
            SearchOption componentSearchOption,
            int numberMinToFind,
            Type typeToFind)
        {
            m_componentSearchPath = componentSearchPath;
            m_componentSearchOption = componentSearchOption;
            m_numberMinToFind = numberMinToFind;
            m_typeToFind = typeToFind;
        }

        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            var go = AttributeValidationUtils.TryConvertObjectToGameObject(
                attributeFieldObj,
                nameof(attributeFieldObj),
                fieldInfo);

            Component[] comps = null;

            bool includeInactive = m_componentSearchOption == SearchOption.IncludeInactive;

            switch (m_componentSearchPath)
            {
                case HierarchyPosition.Self:
                    comps = go.GetComponents(m_typeToFind);
                    break;
                case HierarchyPosition.ParentsAndSelf:
                    // Unity version doesn't seem to work as expected.
                    // comps = go.GetComponentsInParent(m_typeToFind, includeInactive);
                    comps = GetComponentsInParents(go, m_typeToFind, includeInactive);
                    break;
                case HierarchyPosition.ChildsAndSelf:
                    comps = go.GetComponentsInChildren(m_typeToFind, includeInactive);
                    break;
                default:
                    break;
            }

            return comps != null && comps.Length >= m_numberMinToFind;
        }

        private static Component[] GetComponentsInParents(GameObject go, Type type, bool includeInactive)
        {
            var result = new List<Component>();
            var current = go.transform;

            while (current != null)
            {
                if (includeInactive || current.gameObject.activeSelf)
                {
                    result.AddRange(current.gameObject.GetComponents(type));
                }

                current = current.transform.parent;
            }

            return result.ToArray();
        }
    }
}