using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    public class HierarchyValidator : BaseValidator
    {
        private readonly HierarchyPosition m_allowedHierarchy;

        public enum HierarchyPosition
        {
            JustSelf,
            ParentsOnly,
            ChildsOnly,
            ParentsAndSelf,
            ChildsAndSelf,
            Any,
        }

        public HierarchyValidator(HierarchyPosition allowedHierarchy)
        {
            m_allowedHierarchy = allowedHierarchy;
        }

        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            if (ownerObj == null)
            {
                throw new Exception($"{nameof(ownerObj)} cannot be null for {fieldInfo} when {nameof(HierarchyValidator)} is used");
            }

            GameObject fieldGameObject =
                AttributeValidationUtils.TryConvertObjectToGameObject(
                    attributeFieldObj,
                    nameof(attributeFieldObj),
                    fieldInfo);

            GameObject ownerGameObject =
                AttributeValidationUtils.TryConvertObjectToGameObject(
                    ownerObj,
                    nameof(ownerObj),
                    fieldInfo);

            if (fieldGameObject == null || ownerGameObject == null)
            {
                throw new Exception($"[{nameof(HierarchyValidator)}] At least one of the comparing objects is null");
            }

            return CheckParenting(fieldGameObject, ownerGameObject, m_allowedHierarchy);
        }

        private static bool CheckParenting(
            GameObject fieldGameObject,
            GameObject ownerGameObject,
            HierarchyPosition m_selfValidationParam)
        {
            if (m_selfValidationParam == HierarchyPosition.Any)
            {
                return true;
            }
            else if (fieldGameObject.transform == ownerGameObject.transform)
            {
                return
                    m_selfValidationParam == HierarchyPosition.JustSelf ||
                    m_selfValidationParam == HierarchyPosition.ChildsAndSelf ||
                    m_selfValidationParam == HierarchyPosition.ParentsAndSelf;
            }
            else if (
                m_selfValidationParam == HierarchyPosition.ParentsOnly ||
                m_selfValidationParam == HierarchyPosition.ParentsAndSelf)
            {
                return fieldGameObject.GetComponentsInChildren<Transform>(true).Any(t => t == ownerGameObject.transform);
            }
            else if (
                m_selfValidationParam == HierarchyPosition.ChildsOnly ||
                m_selfValidationParam == HierarchyPosition.ChildsAndSelf)
            {
                return ownerGameObject.GetComponentsInChildren<Transform>(true).Any(t => t == fieldGameObject.transform);
            }

            return false;
        }
    }
}