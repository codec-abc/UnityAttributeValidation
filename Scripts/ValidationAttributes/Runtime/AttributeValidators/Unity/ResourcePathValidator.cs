using System;
using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    public class ResourcePathValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            if (attributeFieldObj.GetType() != typeof(string))
            {
                throw new Exception($"[{nameof(ResourcePathValidator)}] field is not a string");
            }

            var content = (string)attributeFieldObj;
            var resource = Resources.Load<UnityEngine.Object>(content);
            return resource != null;
        }
    }
}