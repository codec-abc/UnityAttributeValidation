using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DUDE.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace AttributeValidation
{
    internal class BTPAttributeValidationContext : IValidationContext
    {
        private readonly Dictionary<System.Type, BaseValidator> m_extendedAttributeValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
                // Too many errors
                // { typeof(RangeAttribute), new RangeValidator() },
                // { typeof(DUDE.Core.Attributes.ObjectDatabaseAttribute), new ObjectDatabaseValidator() },
            };

        private readonly Dictionary<System.Type, BaseValidator> m_extendedFieldValidators =
            new Dictionary<System.Type, BaseValidator>()
            {
                { typeof(ObjectReference), new ObjectReferenceValidator() },
                { typeof(DUDE.Core.Unit), new UnitValidator() },
                { typeof(DUDE.Core.Inputs.Axis), new DUDECoreInputsAxisValidator() },
                { typeof(DUDE.Core.Inputs.Input), new DUDECoreInputsInputValidator() },
                { typeof(DUDE.HardwareData), new DUDEHardwareDataValidator() },
                { typeof(DUDE.InputDudeData), new InputDudeDataValidator() },
            };

        public bool ShouldIgnoreObj(object obj, object parentObj, FieldInfo fieldInfo)
        {
            if (ObjectValidationFilters.IsUnityObjImageParameter(parentObj))
            {
                return true;
            }

            return false;
        }

        public IValidationContext.CustomLogicResult CustomLogicForAsset(
            string path,
            Object loadedObj,
            IAssetsToValidateCollection assetsToValidateCollection)
        {
            var isTemplate = IsAssetTemplate(path, loadedObj);

            if (loadedObj is DUDE.Core.Exercise.Exercise exo)
            {
                var result = ManageExercise(path, loadedObj, exo, assetsToValidateCollection);
                if (result.HasValue)
                {
                    return result.Value;
                }
            }

            if (isTemplate)
            {
                return IValidationContext.CustomLogicResult.Ignore;
            }

            return IValidationContext.CustomLogicResult.Process;
        }

        public void AddAssetsToValidate(IAssetsToValidateCollection assetsToValidateCollection)
        {
        }

        private static IValidationContext.CustomLogicResult? ManageExercise(
            string path,
            Object loadedObj,
            DUDE.Core.Exercise.Exercise exo,
            IAssetsToValidateCollection assetsToValidateCollection)
        {
            AddAllParamsInStateForExo(path, exo, exo.LoadingState, assetsToValidateCollection);
            AddAllParamsInStateForExo(path, exo, exo.InitialState, assetsToValidateCollection);

            var states = exo.GetStates();
            foreach (var state in states)
            {
                AddAllParamsInStateForExo(path, exo, state, assetsToValidateCollection);
            }

            return IValidationContext.CustomLogicResult.Ignore;
        }

        public bool ShouldIgnoreEnumerableObj(object enuObj, object obj, object parentObj, FieldInfo fieldInfo)
        {
            if (ObjectValidationFilters.IsUnityObjImageParameter(parentObj))
            {
                return true;
            }

            return false;
        }

        private static void AddAllParamsInStateForExo(
            string path,
            DUDE.Core.Exercise.Exercise exo,
            DUDE.Core.Exercise.BaseState state,
            IAssetsToValidateCollection assetsToValidateCollection)
        {
            var parameters = exo.GetParameters(state);
            foreach (var parameter in parameters)
            {
                var assetToValidate = 
                    new AssetToValidate(
                        parameter, 
                        null, 
                        exo.Name + "/" + state + "/" + parameter,
                        path);

                assetsToValidateCollection.AddIfNotAlreadyInAnalyzeList(assetToValidate);
            }
        }

        private static bool IsAssetTemplate(string path, Object loadedObj)
        {
            var labels = AssetDatabase.GetLabels(loadedObj);
            if (labels != null)
            {
                if (labels.Any(l => l == "AcreosTemplate"))
                {
                    return true;
                }
            }

            return false;
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