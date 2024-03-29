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

            var content = (string)attributeFieldObj;
            var resource = Resources.Load<UnityEngine.Object>(content);
            return resource != null;
        }
    }
}