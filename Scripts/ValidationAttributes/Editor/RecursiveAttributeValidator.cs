using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static AttributeValidation.RecursiveFieldValidation;

namespace AttributeValidation
{
    public class RecursiveAttributeValidator
    {
        public class MemorizedObjectsValidation
        {
            private readonly List<ObjectWithRecursiveValidation> m_memorizedObjectValidationAsList =
                new List<ObjectWithRecursiveValidation>();

            private readonly Dictionary<object, RecursiveAssetValidation> m_memorizedObjectValidationAsDict =
                new Dictionary<object, RecursiveAssetValidation>();

            public class ObjectWithRecursiveValidation
            {
                public object Obj;
                public RecursiveAssetValidation Validation;

                public ObjectWithRecursiveValidation(object asset, RecursiveAssetValidation p)
                {
                    Obj = asset;
                    Validation = p;
                }
            }

            internal (bool, RecursiveAssetValidation) GetEntry(object asset)
            {
                try
                {
                    if (m_memorizedObjectValidationAsDict.TryGetValue(asset, out var returned))
                    {
                        return (true, returned);
                    }

                    return (false, null);
                }
                catch (Exception)
                {
                    var entry =
                        m_memorizedObjectValidationAsList
                        .FirstOrDefault(x => x.Obj == asset);

                    return (true, entry?.Validation);
                }
            }

            internal ObjectWithRecursiveValidation Add(object assetToValidate, RecursiveAssetValidation p)
            {
                m_memorizedObjectValidationAsDict.Add(assetToValidate, p);
                var returned = new ObjectWithRecursiveValidation(assetToValidate, p);
                m_memorizedObjectValidationAsList.Add(returned);
                return returned;
            }

            internal void SetValidation(
                ObjectWithRecursiveValidation entry,
                object assetToValidate,
                RecursiveAssetValidation objValidation)
            {
                entry.Validation = objValidation;
                try
                {
                    m_memorizedObjectValidationAsDict[assetToValidate] = objValidation;
                }
                catch (Exception)
                {
                }
            }
        }

#if DUMP_ENUMERABLE_CONTENT
        private static Dictionary<Type, int> s_countByTypes = new Dictionary<Type, int>();
#endif

        public static Dictionary<AssetToValidate, RecursiveAssetValidation> GetAllInvalidObjectsRecursively(
            AssetToValidate[] assetsToCheck,
            IValidationContext validationContext)
        {
            Dictionary<AssetToValidate, RecursiveAssetValidation> invalidObjects =
                new Dictionary<AssetToValidate, RecursiveAssetValidation>();
#if DUMP_ENUMERABLE_CONTENT
            s_countByTypes.Clear();
#endif

            MemorizedObjectsValidation memorizedObjectValidation = new MemorizedObjectsValidation();

            int currentAssetIndex = 0;

            foreach (AssetToValidate assetToValidate in assetsToCheck)
            {
                var name = assetToValidate.GetAssetNameSafe();

                if (EditorUtility.DisplayCancelableProgressBar(
                    "Checking assets",
                    name,
                    (float)currentAssetIndex / assetsToCheck.Length))
                {
                    break;
                }

                if (assetToValidate != null)
                {
                    if (!invalidObjects.ContainsKey(assetToValidate))
                    {
                        var (hasValidation, validation) = memorizedObjectValidation.GetEntry(assetToValidate.Asset);
                        if (!hasValidation)
                        {
                            var newerEntry = memorizedObjectValidation.Add(assetToValidate.Asset, null);
                            validation = GetRecursiveObjectValidation(assetToValidate, memorizedObjectValidation, validationContext);
                            memorizedObjectValidation.SetValidation(newerEntry, assetToValidate.Asset, validation);
                        }

                        if (validation != null)
                        {
                            if (!validation.IsValid)
                            {
                                invalidObjects.Add(assetToValidate, validation);
                            }
                        }
                    }
                }
                currentAssetIndex++;
            }

#if DUMP_ENUMERABLE_CONTENT
            var dict = s_countByTypes.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            foreach (var kvp in dict)
            {
                UnityEngine.Debug.LogError($"for type {kvp.Key} there is {kvp.Value} element(s) checked for enumerable");
            }
#endif

            return invalidObjects;
        }

        private static (bool, List<RecursiveAssetValidation>) ProcessChildAssets(
            AssetToValidate assetToValidate, 
            MemorizedObjectsValidation memorizedObjects,
            IValidationContext context,
            Dictionary<FieldInfo, RecursiveFieldValidation> dict)
        {
            var obj = assetToValidate.Asset;
            GameObject go = null;
            if (obj is Component comp)
            {
                go = comp.gameObject;
            }
            else if (obj is GameObject gameObj)
            {
                go = gameObj;
            }

            if (go == null)
            {
                return (true, new List<RecursiveAssetValidation>() { });
            }

            var all = go.GetComponentsInChildren<Transform>(true).ToList();
            all.AddRange(go.GetComponentsInParent<Transform>(true));

            var childValidation = new List<RecursiveAssetValidation>();

            foreach (var gameObject in all)
            {
                string hierarchy = GetHierarchyToGoFromGo(gameObject.gameObject, go);
                foreach (var component in gameObject.GetComponents<Component>())
                {
                    if (component != null && !(component is Transform))
                    {
                        var childComp = new AssetToValidate(
                            component,
                            null,
                            $"{hierarchy}" + $"[{component.GetType()}]",
                            assetToValidate.BasePath);

                        AddObjectValidation(childComp, memorizedObjects, childValidation, context);
                    }
                }
            }

            bool areAllChildsValid =
               !childValidation
               .Any((RecursiveAssetValidation objVal) => !objVal.IsValid);

            return (areAllChildsValid, childValidation);
        }

        private static string GetHierarchyToGoFromGo(GameObject childGameObject, GameObject rootGameObject)
        {
            var hierarchy = "";
            var current = childGameObject;
            while (current.transform.parent != null && current.transform.parent.gameObject != rootGameObject)
            {
                hierarchy = current.name + "/" + hierarchy;
                current = current.transform.parent.gameObject;
            }

            if (current != rootGameObject)
            {
                hierarchy = current.name + "/" + hierarchy;
            }

            hierarchy = "/" + rootGameObject.name + "/" + hierarchy;
            return hierarchy;
        }

        private static RecursiveAssetValidation GetRecursiveObjectValidation(
            AssetToValidate assetToValidate,
            MemorizedObjectsValidation memorizedObjects,
            IValidationContext validationContext)
        {
            bool isObjValid = true;

            FieldInfo[] fieldsInfos =
                assetToValidate.Asset.GetType()
                .GetFields(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

            var invalidFields = new Dictionary<FieldInfo, RecursiveFieldValidation>();

            var (areChildrenValids, invalidChildren) = 
                ProcessChildAssets(assetToValidate, memorizedObjects, validationContext, invalidFields);

            foreach (FieldInfo field in fieldsInfos)
            {
                var attributes = field.CustomAttributes;

                var shouldBeAnalyzed = field.Attributes == FieldAttributes.Public;

                if (!shouldBeAnalyzed)
                {
                    foreach (var attribute in attributes)
                    {
                        if (attribute.AttributeType == typeof(SerializeField))
                        {
                            shouldBeAnalyzed = true;
                        }
                    }
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.AttributeType == typeof(NonSerializedAttribute))
                    {
                        shouldBeAnalyzed = false;
                    }
                }

                if (!shouldBeAnalyzed)
                {
                    continue;
                }

                var fieldValidation =
                    GetRecursiveFieldValidation(assetToValidate, field, memorizedObjects, validationContext);

                if (!fieldValidation.IsValid)
                {
                    invalidFields.Add(field, fieldValidation);
                    isObjValid = false;
                }
            }

            return new RecursiveAssetValidation(
                assetToValidate.GetAssetNameSafe(), 
                isObjValid && areChildrenValids, 
                invalidFields,
                invalidChildren);
        }

        private static RecursiveFieldValidation GetRecursiveFieldValidation(
            AssetToValidate assetToValidate,
            FieldInfo fieldInfo,
            MemorizedObjectsValidation memorizedObjects,
            IValidationContext validationContext)
        {
            var assetTypeAsString = assetToValidate.Asset.GetType().ToString();
            var invalidAttributes = new List<InvalidAttribute>();

            var isFieldValid =
                RunValidators(
                    assetToValidate,
                    fieldInfo,
                    validationContext,
                    invalidAttributes);

            isFieldValid &= RunExtentedAttributeValidators(
                assetToValidate,
                fieldInfo,
                validationContext,
                invalidAttributes);

            isFieldValid &= RunExtentedFieldsValidators(
                assetToValidate,
                fieldInfo,
                validationContext,
                invalidAttributes);

            object fieldInfoValue = fieldInfo.GetValue(assetToValidate.Asset);

            var childValidation = new List<RecursiveAssetValidation>();

            var isListOrArray = TryGetListOrArrayType(fieldInfoValue, out var type);

            var fieldToValidate = new 
                AssetToValidate(
                    fieldInfoValue, 
                    assetToValidate, 
                    assetToValidate.AssetName + "/" + fieldInfo.Name,
                    assetToValidate.GetTopParentBasePath());

            AddObjectValidation(
                    fieldToValidate,
                    memorizedObjects,
                    childValidation,
                    validationContext);

            if (isListOrArray && !ShouldTypeBeIgnoreForListOrArray(type, validationContext))
            {
                var asEnumerable = (System.Collections.IEnumerable)fieldInfoValue;

                var count = -1;
                foreach (var elemValue in asEnumerable)
                {
                    count++;
                    if (elemValue != null)
                    {
                        var currentElemToValidate = new AssetToValidate(
                                elemValue,
                                fieldToValidate,
                                fieldToValidate.AssetName + "/" + fieldInfo.Name + $"[{count}]",
                                fieldToValidate.GetTopParentBasePath());

                        AddObjectValidation(
                            currentElemToValidate,
                            memorizedObjects,
                            childValidation,
                            validationContext);
#if DUMP_ENUMERABLE_CONTENT
                        if (s_countByTypes.TryGetValue(type, out int count))
                        {
                            s_countByTypes[type] = count + 1;
                        }
                        else
                        {
                            s_countByTypes[type] = 1;
                        }
#endif
                    }
                }
            }

            bool areAllChildsValid =
                !childValidation
                .Any((RecursiveAssetValidation objVal) => !objVal.IsValid);

            return new RecursiveFieldValidation(
                assetTypeAsString,
                isFieldValid && areAllChildsValid,
                invalidAttributes,
                childValidation);
        }

        private static bool RunValidators(
            AssetToValidate assetToValidate,
            FieldInfo fieldInfo,
            IValidationContext validationContext,
            List<InvalidAttribute> invalidAttributes)
        {
            var isFieldValid = true;

            foreach (var attribute in Attribute.GetCustomAttributes(fieldInfo, typeof(BaseValidatableAttribute)))
            {
                var validatableAttribute = (BaseValidatableAttribute)attribute;

                if (!validatableAttribute.Validate(
                    fieldInfo.GetValue(assetToValidate.Asset),
                    assetToValidate.Asset,
                    fieldInfo,
                    validationContext))
                {
                    invalidAttributes.Add(new InvalidAttribute(validatableAttribute.GetType().Name));
                    isFieldValid = false;
                }
            }

            return isFieldValid;
        }

        private static bool RunExtentedAttributeValidators(
            AssetToValidate assetToValidate,
            FieldInfo fieldInfo,
            IValidationContext validationContext,
            List<InvalidAttribute> invalidAttributes)
        {
            var isFieldValid = true;
            foreach (var extendedAttributeValidator in validationContext.GetExtendedAttributeValidators())
            {
                var typeOfAttribute = extendedAttributeValidator.Key;
                var validatorForAttribute = extendedAttributeValidator.Value;

                var attribute2 = Attribute.GetCustomAttribute(fieldInfo, typeOfAttribute);
                if (attribute2 != null)
                {
                    if (validationContext.ShouldIgnoreObj(
                        fieldInfo.GetValue(assetToValidate.Asset),
                        assetToValidate.Asset,
                        fieldInfo))
                    {
                        continue;
                    }

                    if (!validatorForAttribute.Validate(
                        fieldInfo.GetValue(assetToValidate.Asset),
                        assetToValidate.Asset,
                        fieldInfo))
                    {
                        invalidAttributes.Add(new InvalidAttribute(validatorForAttribute.GetType().Name));
                        isFieldValid = false;
                    }
                }
            }

            return isFieldValid;
        }

        private static bool RunExtentedFieldsValidators(
            AssetToValidate assetToValidate,
            FieldInfo fieldInfo,
            IValidationContext validationContext,
            List<InvalidAttribute> invalidAttributes)
        {
            var isFieldValid = true;

            foreach (var fieldValidator in validationContext.GetExtendedFieldValidators())
            {
                var typeOfField = fieldValidator.Key;
                var validator = fieldValidator.Value;

                if (validationContext.ShouldIgnoreObj(
                       fieldInfo.GetValue(assetToValidate.Asset),
                       assetToValidate.Asset,
                       fieldInfo))
                {
                    continue;
                }

                if (IsSameOrSubclass(typeOfField, fieldInfo.FieldType))
                {
                    if (!validator.Validate(
                            fieldInfo.GetValue(assetToValidate.Asset),
                            assetToValidate.Asset,
                            fieldInfo))
                    {
                        invalidAttributes.Add(new InvalidAttribute(validator.GetType().Name));
                        isFieldValid = false;
                    }
                }
            }

            return isFieldValid;
        }

        private static void AddObjectValidation(
            AssetToValidate assetToValidate,
            MemorizedObjectsValidation memorizedObjects,
            List<RecursiveAssetValidation> listToAppend,
            IValidationContext validationContext)
        {
            var (hasValidation, validation) = memorizedObjects.GetEntry(assetToValidate.Asset);
            if (!hasValidation)
            {
                var newerEntry = memorizedObjects.Add(assetToValidate.Asset, null);
                validation = GetRecursiveObjectValidation(assetToValidate, memorizedObjects, validationContext);
                memorizedObjects.SetValidation(newerEntry, assetToValidate, validation);
            }

            if (validation != null && !validation.IsValid)
            {
                listToAppend.Add(validation);
            }
        }

        private static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        private static bool IsUnityObjAlive(UnityEngine.Object obj)
        {
#pragma warning disable RS0030
            return obj;
#pragma warning restore
        }

        public static bool TryGetListOrArrayType(object obj, out Type type)
        {
            type = null;
            if (obj == null)
            {
                return false;
            }
            var isArray = obj.GetType().IsArray;

            if (isArray)
            {
                type = obj.GetType().GetElementType();
                return true;
            }

            var isList = IsList(obj);

            if (isList)
            {
                type = obj.GetType().GenericTypeArguments[0];
                return true;
            }

            return false;
        }

        public static bool IsList(object o)
        {
            if (o == null)
            {
                return false;
            }
            return o is System.Collections.IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsPrimitiveType(Type type)
        {
            if (type == typeof(string))
            {
                return true;
            }

            if (type == typeof(bool))
            {
                return true;
            }

            if (type == typeof(float))
            {
                return true;
            }

            if (type == typeof(int))
            {
                return true;
            }

            if (type == typeof(uint))
            {
                return true;
            }

            if (type == typeof(char))
            {
                return true;
            }

            if (type == typeof(byte))
            {
                return true;
            }

            if (type == typeof(sbyte))
            {
                return true;
            }

            if (type == typeof(double))
            {
                return true;
            }

            return false;
        }

        public static bool IsUnityBasicTypes(Type type)
        {
            if (type == typeof(UnityEngine.Vector2))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Vector3))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Vector4))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Vector2Int))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Vector3Int))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Quaternion))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Matrix4x4))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Rect))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Color))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Color32))
            {
                return true;
            }

            return false;
        }

        public static bool ShouldTypeBeIgnoreForListOrArray(Type type, IValidationContext context)
        {
            if (IsPrimitiveType(type))
            {
                return true;
            }

            if (IsUnityBasicTypes(type))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Material))
            {
                return true;
            }

            if (type == typeof(UnityEngine.Rigidbody))
            {
                return true;
            }

            if (type.IsSubclassOf(typeof(UnityEngine.Collider)))
            {
                return true;
            }

            if (type.IsSubclassOf(typeof(UnityEngine.Texture)))
            {
                return true;
            }

            if (type.IsSubclassOf(typeof(UnityEngine.Renderer)))
            {
                return true;
            }

            return context.ShouldIgnoreType(type);
        }
    }
}