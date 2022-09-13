using System.IO;
using System.Reflection;
using UnityEditor;

namespace AttributeValidation
{
    public class AssetKindValidator : BaseValidator
    {
        public enum AssetKind
        {
            SceneObject,
            AnyButSceneObject,
        }

        private readonly AssetKind m_assetKind;

        public AssetKindValidator(AssetKind assetKind)
        {
            m_assetKind = assetKind;
        }

        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
#if UNITY_EDITOR
            if (attributeFieldObj == null)
            {
                return false;
            }

            var unityObj = AttributeValidationUtils.TryConvertToUnityObject(
                attributeFieldObj,
                nameof(attributeFieldObj),
                fieldInfo);

            if (unityObj == null)
            {
                return false;
            }

            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(unityObj, out string guid, out long _))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                return CheckPath(fieldInfo, unityObj, path);
            }

            var assetPath = AssetDatabase.GetAssetOrScenePath(unityObj);
            return CheckPath(fieldInfo, unityObj, assetPath);
#else
        return true;
#endif
        }

        private bool CheckPath(FieldInfo fieldInfo, UnityEngine.Object unityObj, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var extension = Path.GetExtension(path);

            if (extension.ToLower() == ".unity")
            {
                return m_assetKind == AssetKind.SceneObject;
            }

            return m_assetKind == AssetKind.AnyButSceneObject;
        }
    }
}