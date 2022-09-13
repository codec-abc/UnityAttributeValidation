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

            if (attributeFieldObj.GetType() != typeof(ObjectReference))
            {
                throw new System.Exception($"[{nameof(ObjectReferenceValidator)}] field is not a {typeof(ObjectReference)} for field {fieldInfo}.");
            }

            var value = (ObjectReference)attributeFieldObj;

            var resourcesPath = value.ResourcesPath;
            var resource = Resources.Load<UnityEngine.Object>(resourcesPath);
            return resource != null;
        }
    }
}