using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AttributeValidation
{
    public class AttributeValidationScript : IAssetsToValidateCollection
    {
        private readonly Stopwatch m_stopwatch = Stopwatch.StartNew();

        public bool ResultBeingCalculated { get; private set; }

        private readonly AttributeValidationConfig m_validationParameters;
        private readonly IValidationContext m_validationContext;
        private List<AssetToValidate> m_allAssetsFound;

        private List<string> m_allScenesPaths;

        private bool m_logInConsole;

        private Exception m_runException = null;

        public AttributeValidationScript(
            AttributeValidationConfig validationParameters,
            IValidationContext validationContext)
        {
            m_validationParameters = validationParameters;
            m_validationContext = validationContext;
        }

        public AttributeValidationResult RunValidationProcess(bool logInConsole)
        {
            m_logInConsole = logInConsole;
            ResultBeingCalculated = true;
            GetAllAssetsToValidate();
            m_validationContext.AddAssetsToValidate(this);
            AttributeValidationResult result = ValidateAllAssets();
            if (m_validationParameters.AnalyzeScenes)
            {
                GetAllObjectFromScenes(result);
            }
            if (m_logInConsole)
            {
                UnityEngine.Debug.LogError(result.ToJsonResult());
            }
            return result;
        }

        public AttributeValidationResult RunValidationProcess(bool logInConsole, List<string> assetToValidatePaths)
        {
            m_logInConsole = logInConsole;
            ResultBeingCalculated = true;
            m_allAssetsFound = new List<AssetToValidate>();
            foreach (var elem in assetToValidatePaths)
            {
                AddToListAllAssetsAtPath(elem);
            }
            AttributeValidationResult result = ValidateAllAssets();
            if (m_validationParameters.AnalyzeScenes)
            {
                GetAllObjectFromScenes(result);
            }
            if (m_logInConsole)
            {
                UnityEngine.Debug.LogError(result.ToJsonResult());
            }
            return result;
        }

        private AttributeValidationResult ValidateAllAssets()
        {
            m_stopwatch.Restart();
            AttributeValidationResult result;
            try
            {
                if (m_runException != null)
                {
                    result = AttributeValidationResult.InternalErrorAttributeValidationResult(m_runException);
                }
                else
                {
                    var dict = RecursiveAttributeValidator.GetAllInvalidObjectsRecursively(
                        m_allAssetsFound.ToArray(),
                        m_validationContext);

                    result = AttributeValidationResult.NormalAttributeValidationResult(dict);
                }
            }
            catch (Exception ex)
            {
                result = AttributeValidationResult.InternalErrorAttributeValidationResult(ex);
            }

            ResultBeingCalculated = false;
            m_runException = null;
            m_stopwatch.Stop();
            EditorUtility.ClearProgressBar();
            UnityEngine.Debug.LogWarning($"Time to get all invalid objects : {m_stopwatch.Elapsed.Seconds} seconds");

            return result;
        }

        private void CancelOperation()
        {
            UnityEngine.Debug.LogError("Operation canceled");
            EditorUtility.ClearProgressBar();
            ResultBeingCalculated = false;
        }

        private void GetAllAssetsToValidate()
        {
            m_stopwatch.Restart();
            try
            {
                string[] allAssetsPaths = AssetDatabase.GetAllAssetPaths();
                m_allAssetsFound = new List<AssetToValidate>();

                m_allScenesPaths = new List<string>();

                int currentAssetIndex = 0;

                foreach (string path in allAssetsPaths)
                {
                    if (!ShouldIgnorePath(path))
                    {
                        if (EditorUtility.DisplayCancelableProgressBar(
                            "Searching for all assets",
                            path,
                            (float)currentAssetIndex / allAssetsPaths.Length))
                        {
                            CancelOperation();
                            break;
                        }

                        // As we can't recover all the asset is a scene by simply using AssetDatabase.LoadAllAssetsAtPath(),
                        // We have to do it manually
                        if (path.EndsWith(AttributeValidationConfig.SCENE_FILE_EXTENSION))
                        {
                            m_allScenesPaths.Add(path);
                        }
                        else
                        {
                            AddToListAllAssetsAtPath(path);
                        }
                    }

                    currentAssetIndex++;
                }
            }
            catch (Exception e)
            {
                m_runException = e;
                UnityEngine.Debug.LogError(e);
            }

            EditorUtility.ClearProgressBar();

            m_stopwatch.Stop();
            UnityEngine.Debug.LogWarning($"Time to find all assets in Folders : {m_stopwatch.Elapsed.Seconds} seconds");

            m_stopwatch.Restart();
        }

        private void AddToListAllAssetsAtPath(string path)
        {
            if (ShouldIgnorePath(path))
            {
                return;
            }

            UnityEngine.Object loadedObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

            var customLogicResult = m_validationContext.CustomLogicForAsset(path, loadedObj, this);

            if (customLogicResult == IValidationContext.CustomLogicResult.Process)
            {
                if (loadedObj is GameObject)
                {
                    ProcessChildAssets(path, loadedObj as GameObject);
                }
                else
                {
                    foreach (UnityEngine.Object ob in AssetDatabase.LoadAllAssetsAtPath(path))
                    {
                        if (ob != null)
                        {
                            AssetToValidate childComp = new AssetToValidate(ob, path, "/");
                            AddIfNotAlreadyInAnalyzeList(childComp);
                        }
                    }
                }
            }
        }

        private void GetAllObjectFromScenes(AttributeValidationResult result)
        {
            var sceneIndex = -1;
            foreach (var sceneToRun in m_allScenesPaths)
            {
                sceneIndex++;
                if (EditorUtility.DisplayCancelableProgressBar(
                    "Searching for all assets in scenes",
                    sceneToRun,
                    (float)sceneIndex / m_allScenesPaths.Count))
                {
                    CancelOperation();
                }
                else
                {
                    _ = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                    Scene currentScene = EditorSceneManager.OpenScene(sceneToRun);
                    try
                    {
                        m_allAssetsFound.Clear();
                        foreach (var rootGameObject in currentScene.GetRootGameObjects())
                        {
                            ProcessChildAssets(currentScene.path, rootGameObject);
                        }
                        AggregateResults(ref result);
                    }
                    catch (Exception ex)
                    {
                        m_runException = ex;
                        UnityEngine.Debug.LogError(ex);
                    }
                }
            }
            EditorUtility.ClearProgressBar();
            m_stopwatch.Stop();

            UnityEngine.Debug.LogWarning($"Time to find all assets in Scenes : {m_stopwatch.Elapsed.Seconds} seconds");
        }

        private void AggregateResults(ref AttributeValidationResult result)
        {
            try
            {
                if (result.ValidationResultKind == AttributeValidationResult.ResultKind.InternalError)
                {
                    return;
                }

                if (m_runException != null)
                {
                    result.InternalError = m_runException;
                    result.ValidationResultKind = AttributeValidationResult.ResultKind.InternalError;
                }
                else
                {
                    var dict = RecursiveAttributeValidator.GetAllInvalidObjectsRecursively(
                        m_allAssetsFound.ToArray(),
                        m_validationContext);

                    if (result.ValidationResultKind != AttributeValidationResult.ResultKind.Success &&
                        dict.Count > 0)
                    {
                        result.ValidationResultKind = AttributeValidationResult.ResultKind.Failure;
                    }

                    result.Errors.AddRange(AttributeValidationResult.FlattenDictionary(dict));
                    result.AddRange(dict);
                }
            }
            catch (Exception ex)
            {
                result = AttributeValidationResult.InternalErrorAttributeValidationResult(ex);
            }
        }

        private void ProcessChildAssets(string path, GameObject rootGameObject)
        {
            var childs = rootGameObject.GetComponentsInChildren<Transform>(true);
            foreach (var childGameObject in childs)
            {
                string hierarchy = GetHierarchyToGoFromGo(childGameObject.gameObject, rootGameObject);
                foreach (var component in childGameObject.GetComponents<Component>())
                {
                    if (component != null && !(component is Transform))
                    {
                        var childComp = new AssetToValidate(
                            component,
                            path,
                            $"{hierarchy}" + $"[{component.GetType()}]");

                        AddIfNotAlreadyInAnalyzeList(childComp);
                    }
                }
            }
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

        private bool ShouldIgnorePath(string path)
        {
            foreach (string folderToIgnore in m_validationParameters.FolderPathsToIgnore)
            {
                if (path.StartsWith(folderToIgnore))
                {
                    return true;
                }
            }

            return IsFileExtensionIgnored(path);
        }

        private bool IsFileExtensionIgnored(string path)
        {
            foreach (string extensionToIgnore in m_validationParameters.FileExtensionsToIgnore)
            {
                if (path.EndsWith(extensionToIgnore))
                {
                    return true;
                }
            }
            return false;
        }

        private static void StringifyObjectValidation(
            List<RecursiveAssetValidation> validation,
            StringBuilder builder,
            int indentLevel)
        {
            foreach (var invalidObject in validation)
            {
                foreach (var fieldInfo in invalidObject.InvalidFields.Keys)
                {
                    _ = builder.AppendLine(
                        Concat(AttributeValidationConfig.SPACE_INDENT, indentLevel) +
                        $"=> Field [{fieldInfo.Name}] invalid attributes : ");

                    RecursiveFieldValidation fieldValidation = invalidObject.InvalidFields[fieldInfo];

                    StringifyFieldValidation(fieldValidation, builder, indentLevel + 1);
                }
            }
        }

        private static void StringifyFieldValidation(
            RecursiveFieldValidation validation,
            StringBuilder builder,
            int indentLevel)
        {
            if (validation.IsValid)
            {
                _ = builder.AppendLine(
                    Concat(AttributeValidationConfig.SPACE_INDENT, indentLevel) +
                    "- Is Valid");
            }
            else
            {
                foreach (var invalidAttribute in validation.InvalidAttributes)
                {
                    _ = builder.AppendLine(
                        Concat(AttributeValidationConfig.SPACE_INDENT, indentLevel) +
                        $"=> {invalidAttribute.GetAttributeType()}");
                }
                if (validation.IsFieldNotValue && validation.ChildsValidations.Count != 0)
                {
                    _ = builder.AppendLine(Concat(AttributeValidationConfig.SPACE_INDENT, indentLevel) + $"Invalid Children : ");
                    StringifyObjectValidation(validation.ChildsValidations, builder, indentLevel + 1);
                }
            }
        }

        private static string Concat(string st, int times)
        {
            string str = string.Empty;
            for (int i = 0; i < times; i++)
            {
                str += st;
            }
            return str;
        }

        public void AddIfNotAlreadyInAnalyzeList(AssetToValidate assetToValidate)
        {
            if (!m_allAssetsFound.Any(c => ReferenceEquals(assetToValidate.Asset, c.Asset)))
            {
                m_allAssetsFound.Add(assetToValidate);
            }
        }
    }
}