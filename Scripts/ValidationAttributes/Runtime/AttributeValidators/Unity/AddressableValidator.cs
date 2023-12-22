using System;
using System.Reflection;
//using UnityEngine.AddressableAssets;

namespace AttributeValidation
{
	public class AddressableValidator : BaseValidator
	{
		//public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
		//{
		//    if (attributeFieldObj == null)
		//    {
		//        return false;
		//    }

		//    return IsAddressValidWeak((string)attributeFieldObj);
		//}

		//public static bool IsAddressValid(string path)
		//{
		//    try
		//    {
		//        var load = Addressables.LoadAssetAsync<UnityEngine.Object>(path);
		//        var obj = load.WaitForCompletion();
		//        return obj != null;
		//    }
		//    catch (Exception)
		//    {
		//        return false;
		//    }
		//}

		//public static bool IsAddressValidWeak(string path)
		//{
		//    if (string.IsNullOrWhiteSpace(path))
		//    {
		//        return true;
		//    }

		//    try
		//    {
		//        var load = Addressables.LoadAssetAsync<UnityEngine.Object>(path);
		//        var obj = load.WaitForCompletion();
		//        return obj != null;
		//    }
		//    catch (Exception)
		//    {
		//        return false;
		//    }
		//}
		public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
		{
			throw new NotImplementedException();
		}
	}
}