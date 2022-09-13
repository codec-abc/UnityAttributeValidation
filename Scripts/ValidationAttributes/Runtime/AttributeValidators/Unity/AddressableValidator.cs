using System;
using System.Reflection;
using UnityEngine.AddressableAssets;

namespace AttributeValidation
{
    public class AddressableValidator : BaseValidator
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
            try
            {
                var load = Addressables.LoadAssetAsync<UnityEngine.Object>(attributeFieldObj);
                var obj = load.WaitForCompletion();
                return obj != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}