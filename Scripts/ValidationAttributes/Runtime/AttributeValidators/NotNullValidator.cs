using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    public class NotNullValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj != null && !attributeFieldObj.GetType().IsValueType)
            {
                if (attributeFieldObj is UnityEngine.Object unityObj)
                {
                    var returned = IsUnityObjectNotNull(unityObj);
                    return returned;
                }
                return true;
            }
            else if (attributeFieldObj == null)
            {
                return false;
            }

            var msg = $"Cannot apply {typeof(NotNullValidator)} to variables of type {attributeFieldObj.GetType()}";
            throw new System.Exception(msg);
        }

        private static bool IsUnityObjectNotNull(Object unityObj)
        {
            var result = unityObj != null;
            return result;
        }
    }
}