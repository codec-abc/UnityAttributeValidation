using System.Reflection;
using DUDE.Core.Attributes;
using UnityEngine;

namespace AttributeValidation
{
    public class ObjectReferenceValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            var value = (ObjectReference)attributeFieldObj;

            var resourcesPath = value.ResourcesPath;

            if (string.IsNullOrEmpty(resourcesPath))
            {
                return true;
            }

            var resource = Resources.Load<UnityEngine.Object>(resourcesPath);
            return resource != null;
        }
    }
}