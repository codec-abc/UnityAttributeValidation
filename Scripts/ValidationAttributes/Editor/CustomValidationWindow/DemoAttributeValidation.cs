using System.Collections.Generic;
using UnityEditor;

namespace AttributeValidation
{
    internal static class DemoAttributeValidation
    {
        //[MenuItem("Assets/Attribute Validation/Run Attribute Validator (no scene)")]
        //public static void RunAttributeValidationForSelectionNoScene()
        //{
        //    var analyzeScenes = false;
        //    RunAttributeValidationForSelection(analyzeScenes);
        //}

        //[MenuItem("Assets/Attribute Validation/Run Attribute Validator (all scenes)")]
        //public static void RunAttributeValidationForSelectionAllScenes()
        //{
        //    var analyzeScenes = true;
        //    RunAttributeValidationForSelection(analyzeScenes);
        //}

        //private static void RunAttributeValidationForSelection(bool analyzeScenes)
        //{
        //    var guids = Selection.assetGUIDs;
        //    var pathsToAnalyze =    .GetSelectionPaths(guids);
        //    var pathsToIgnore = GetDemoIgnorePath();
        //    var fileExtToIgnore = GetIgnoredFileTypeExtensions();

        //    var config = new AttributeValidationConfig(
        //        pathsToIgnore,
        //        fileExtToIgnore,
        //        analyzeScenes);

        //    var validationScript = new AttributeValidationScript(config, new DemoAttributeValidationContext());

        //    var result = validationScript.RunValidationProcess(false, pathsToAnalyze);

        //    if (result.ValidationResultKind != AttributeValidationResult.ResultKind.Success)
        //    {
        //        UnityEngine.Debug.LogError(result.ToJsonResult());
        //        AttributeValidationWindow.ShowErrorDialog();
        //    }
        //    else
        //    {
        //        AttributeValidationWindow.ShowGoodDialog();
        //    }
        //}

        public static AttributeValidationResult RunDemoAttributeValidatorConfig(bool analyzeScenes)
        {
            List<string> pathsToIgnore = GetDemoIgnorePath();

            var fileExtToIgnore = GetIgnoredFileTypeExtensions();

            var config = new AttributeValidationConfig(
                pathsToIgnore,
                fileExtToIgnore,
                analyzeScenes);

            var validationScript = new AttributeValidationScript(config, new DemoAttributeValidationContext());

            var result = validationScript.RunValidationProcess(false);

            return result;
        }

        private static List<string> GetDemoIgnorePath()
        {
            return new List<string>()
            {
                "Packages/",
                "Assets/_PLUGINS",
                "Assets/Plugins",
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