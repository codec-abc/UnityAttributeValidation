using System.Collections.Generic;
using UnityEditor;

namespace AttributeValidation
{
    internal static class BTPAttributeValidation
    {
        [MenuItem("Assets/BTP Validation/Run Attribute Validator (no scene)")]
        public static void RunAttributeValidationForSelectionNoScene()
        {
            var analyzeScenes = false;
            RunAttributeValidationForSelection(analyzeScenes);
        }

        [MenuItem("Assets/BTP Validation/Run Attribute Validator (all scenes)")]
        public static void RunAttributeValidationForSelectionAllScenes()
        {
            var analyzeScenes = true;
            RunAttributeValidationForSelection(analyzeScenes);
        }

        private static void RunAttributeValidationForSelection(bool analyzeScenes)
        {
            var guids = Selection.assetGUIDs;
            var pathsToAnalyze = ValidationUtils.GetSelectionPaths(guids);
            var pathsToIgnore = GetBTPIgnorePath();
            var fileExtToIgnore = GetIgnoredFileTypeExtensions();

            var config = new AttributeValidationConfig(
                pathsToIgnore,
                fileExtToIgnore,
                analyzeScenes);

            var validationScript = new AttributeValidationScript(config, new BTPAttributeValidationContext());

            var result = validationScript.RunValidationProcess(false, pathsToAnalyze);

            if (result.ValidationResultKind != AttributeValidationResult.ResultKind.Success)
            {
                UnityEngine.Debug.LogError(result.ToJsonResult());
                AttributeValidationWindow.ShowErrorDialog();
            }
            else
            {
                AttributeValidationWindow.ShowGoodDialog();
            }
        }

        public static AttributeValidationResult RunBTPAttributeValidatorConfig()
        {
            List<string> pathsToIgnore = GetBTPIgnorePath();

            var fileExtToIgnore = GetIgnoredFileTypeExtensions();

            var analyzeScenes = true;

            var config = new AttributeValidationConfig(
                pathsToIgnore,
                fileExtToIgnore,
                analyzeScenes);

            var validationScript = new AttributeValidationScript(config, new BTPAttributeValidationContext());

            var result = validationScript.RunValidationProcess(false);

            return result;
        }

        private static List<string> GetBTPIgnorePath()
        {
            return new List<string>()
            {
                "Packages/",
                "Assets/_PROJECT/0DAsset/Resources/Module/Templates",
                "Assets/_DUDE_DATAS",
                "Assets/_DUDE_Vegetation",
                "Assets/_PLUGINS",
                "Assets/_DUDE_Circulation",
                "Assets/_PROJECT/DiggingPrecomputedData",
                "Assets/Plugins",
                "Assets/_DUDE_Digging",
                "Assets/Samples/DUDE.Vehicle",
                "Assets/TestValidator",
            };
        }

        private static List<string> GetIgnoredFileTypeExtensions()
        {
            return new List<string>()
            {
                ".cs",
                ".png",
                ".fbx",
                ".FBX",
                ".mp3",
                ".wav",
                ".mat",
                ".mp4",
                ".tif",
                ".tga",
                ".bmp",
                ".txt",
                ".json",
            };
        }
    }
}