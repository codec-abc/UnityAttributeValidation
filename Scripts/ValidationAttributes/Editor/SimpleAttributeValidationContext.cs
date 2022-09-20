using System.Collections.Generic;
using System.Reflection;
using DUDE.Core.Attributes;
using UnityEngine;

namespace AttributeValidation
{
    internal class SimpleAttributeValidationContext : IValidationContext
    {
        private readonly Dictionary<System.Type, BaseValidator> m_extendedAttributeValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
                // { typeof(RangeAttribute), new RangeValidator() },
                { typeof(DUDE.Core.Attributes.ObjectDatabaseAttribute), new ObjectDatabaseValidator() },
            };

        private readonly Dictionary<System.Type, BaseValidator> m_extendedFieldValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
                { typeof(ObjectReference), new ObjectReferenceValidator() },
            };

        public bool ShouldIgnoreObj(object obj, object parentObj, FieldInfo fieldInfo)
        {
            return false;
        }

        public bool ShouldIgnoreEnumerableObj(object enuObj, object obj, object parentObj, FieldInfo fieldInfo)
        {
            return false;
        }

        public void AddAssetsToValidate(IAssetsToValidateCollection attributeValidationScript)
        {
        }

        public IValidationContext.CustomLogicResult CustomLogicForAsset(
            string path,
            Object loadedObj,
            IAssetsToValidateCollection assetsToValidateCollection)
        {
            return IValidationContext.CustomLogicResult.Process;
        }

        public IReadOnlyDictionary<System.Type, BaseValidator> GetExtendedAttributeValidators()
        {
            return m_extendedAttributeValidators;
        }

        public IReadOnlyDictionary<System.Type, BaseValidator> GetExtendedFieldValidators()
        {
            return m_extendedFieldValidators;
        }

        public bool ShouldIgnoreType(System.Type type)
        {
            if (type == typeof(AwesomeTechnologies.Vegetation.PersistentStorage.PersistentVegetationItem))
            {
                return true;
            }

            if (type == typeof(AwesomeTechnologies.Vegetation.PersistentStorage.PersistentVegetationCell))
            {
                return true;
            }

            if (type == typeof(AwesomeTechnologies.Vegetation.PersistentStorage.SourceCount))
            {
                return true;
            }

            if (type == typeof(AwesomeTechnologies.Utility.BVHTree.LBVHNODE))
            {
                return true;
            }

            if (type == typeof(AwesomeTechnologies.Utility.BVHTree.LBVHTriangle))
            {
                return true;
            }

            if (type == typeof(DUDE.Circulation.RoadNetwork.Modifier.BaseModifier))
            {
                return true;
            }

            if (type == typeof(UnityEngine.TextCore.Glyph))
            {
                return true;
            }

            if (type == typeof(TMPro.TMP_Glyph))
            {
                return true;
            }

            if (type == typeof(TMPro.TMP_Character))
            {
                return true;
            }

            if (type == typeof(TMPro.TMP_FontWeightPair))
            {
                return true;
            }

            if (type == typeof(UnityEngine.TextCore.GlyphRect))
            {
                return true;
            }

            if (type == typeof(EasyRoads3Dv3.ERTerrainData))
            {
                return true;
            }

            if (type == typeof(EasyRoads3Dv3.ERTerrainChange))
            {
                return true;
            }

            if (type == typeof(EasyRoads3Dv3.ZIndexArray))
            {
                return true;
            }

            if (type == typeof(EasyRoads3Dv3.ERSORoadExt))
            {
                return true;
            }

            return false;
        }
    }
}