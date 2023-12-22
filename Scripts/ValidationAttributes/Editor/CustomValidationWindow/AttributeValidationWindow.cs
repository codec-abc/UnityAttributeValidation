using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AttributeValidation
{
    public class AttributeValidationWindow : EditorWindow
    {
        private readonly int m_spaceBetweenElements = 4;
        private readonly float m_separationLineWidth = 2;

        private readonly Dictionary<object, FoldOutState> m_foldOutStateByObjects =
            new Dictionary<object, FoldOutState>();

        [System.NonSerialized]
        private AttributeValidationScriptableConfig m_parameters;

        private AttributeValidationScript m_validationScript;
        private AttributeValidationResult m_validationResult;

        private class FoldOutState
        {
            public bool IsFoldOut;

            [AttributeValidation.NotNull]
            public string Name;
        }

        [System.NonSerialized]
        private Vector2 m_currentScrollPosition;

        [System.NonSerialized]
        private bool m_resultFoldout = false;

        [System.NonSerialized]
        private Color m_lineColor = new Color(0.3515625f, 0.3515625f, 0.3515625f);

        [MenuItem("Attribute Validation/Custom Profile")]
        public static void DrawValidationWindow()
        {
            EditorWindow validationWin = GetWindow<AttributeValidationWindow>();
            validationWin.titleContent = new GUIContent("Attribute Validation");
        }

        [MenuItem("Attribute Validation/Demo Profile (With Scenes)")]
        public static void RunDemoAttributeValidationOnAssetsWithScenes()
        {
            var result = DemoAttributeValidation.RunDemoAttributeValidatorConfig(true);
            if (result.ValidationResultKind != AttributeValidationResult.ResultKind.Success)
            {
                var errorMsg = result.ToJsonResult();
                Debug.LogError(errorMsg);
                ShowErrorDialog();
            }
            else
            {
                ShowGoodDialog();
            }
        }

        [MenuItem("Attribute Validation/Demo Profile (No Scene)")]
        public static void RunDemoAttributeValidationOnAssetsNoScene()
        {
            var result = DemoAttributeValidation.RunDemoAttributeValidatorConfig(false);
            if (result.ValidationResultKind != AttributeValidationResult.ResultKind.Success)
            {
                var errorMsg = result.ToJsonResult();
                Debug.LogError(errorMsg);
                ShowErrorDialog();
            }
            else
            {
                ShowGoodDialog();
            }
        }

        public static void ShowErrorDialog()
        {
            _ = EditorUtility.DisplayDialog(
                            "❌ Attribute validation errors",
                            "❌ There is errors in your assets. Look at the console to get a description.",
                            "OK");
        }

        public static void ShowGoodDialog()
        {
            _ = EditorUtility.DisplayDialog(
                            "✔️ Attribute validation no error",
                            "✔️ There is no error in your assets. All good.",
                            "OK");
        }

        protected void OnGUI()
        {
            DisplayTopPane();
            DisplayResult();
        }

        private void DisplayTopPane()
        {
            // Parameter Field & Button
            m_parameters = EditorGUILayout.ObjectField(m_parameters, typeof(AttributeValidationScriptableConfig), true) as AttributeValidationScriptableConfig;

            GUIContent buttonContent = new GUIContent("Find invalid objects");
            using (new EditorGUI.DisabledScope(m_parameters == null || (m_validationScript != null && m_validationScript.ResultBeingCalculated)))
            {
                if (GUILayout.Button(buttonContent))
                {
                    m_validationScript = new AttributeValidationScript(m_parameters.Config, new SimpleAttributeValidationContext());
                    m_foldOutStateByObjects.Clear();
                    m_validationResult = m_validationScript.RunValidationProcess(true);
                }
            }

            // Separation Line
            Rect separationLine = new Rect(
                x: m_spaceBetweenElements,
                y: (EditorGUIUtility.singleLineHeight + m_spaceBetweenElements) * 2,
                width: position.width - m_spaceBetweenElements * 2,
                height: m_separationLineWidth);

            EditorGUI.DrawRect(separationLine, m_lineColor);
        }

        private void DisplayResult()
        {
            if (m_validationResult != null)
            {
                // Result Display
                m_currentScrollPosition = GUILayout.BeginScrollView(m_currentScrollPosition);
                m_resultFoldout = EditorGUILayout.Foldout(m_resultFoldout, "Invalid Fields");

                if (m_resultFoldout)
                {
                    EditorGUI.indentLevel++;
                    foreach (var invalidAsset in m_validationResult.InvalidAssets.Keys)
                    {
                        DisplayAssetValidation(
                            $"{invalidAsset.GetAssetNameSafe()}  -  Path : {invalidAsset.AssetName}",
                            m_validationResult.InvalidAssets[invalidAsset]);

                        EditorGUILayout.Separator();
                    }
                    EditorGUI.indentLevel--;
                }
                GUILayout.EndScrollView();
            }
        }

        private void DisplayAssetValidation(string assetName, RecursiveAssetValidation invalidAsset)
        {
            var foldoutState = GetOrCreateFoldoutState(assetName, invalidAsset);

            foldoutState.IsFoldOut = EditorGUILayout.Foldout(foldoutState.IsFoldOut, foldoutState.Name);
            if (foldoutState.IsFoldOut)
            {
                EditorGUI.indentLevel++;
                foreach (FieldInfo fieldInfo in invalidAsset.InvalidFields.Keys)
                {
                    DisplayFieldValidation(fieldInfo.Name, invalidAsset.InvalidFields[fieldInfo]);
                }

                // TODO : handle child transforms
                EditorGUI.indentLevel--;
            }
        }

        private FoldOutState GetOrCreateFoldoutState(string assetName, object obj)
        {
            if (m_foldOutStateByObjects.TryGetValue(obj, out FoldOutState state))
            {
                return state;
            }
            else
            {
                state = new FoldOutState
                {
                    IsFoldOut = true,
                    Name = assetName,
                };
                m_foldOutStateByObjects.Add(obj, state);
                return state;
            }
        }

        private void DisplayFieldValidation(string fieldName, RecursiveFieldValidation invalidField)
        {
            var foldoutState = GetOrCreateFoldoutState($"Field [{fieldName}]", invalidField);
            foldoutState.IsFoldOut = EditorGUILayout.Foldout(foldoutState.IsFoldOut, foldoutState.Name);
            if (foldoutState.IsFoldOut)
            {
                EditorGUI.indentLevel++;

                // Display Attributes
                foldoutState = GetOrCreateFoldoutState("Invalid Attributes : ", invalidField.InvalidAttributes);
                foldoutState.IsFoldOut = EditorGUILayout.Foldout(foldoutState.IsFoldOut, foldoutState.Name);
                if (foldoutState.IsFoldOut)
                {
                    EditorGUI.indentLevel++;
                    foreach (var invalidAttribute in invalidField.InvalidAttributes)
                    {
                        EditorGUILayout.LabelField($"- {invalidAttribute.GetAttributeType()}");
                    }
                    EditorGUI.indentLevel--;
                }

                // Display ChildValidations
                if (invalidField.IsFieldNotValue && invalidField.ChildsValidations.Count != 0)
                {
                    EditorGUILayout.Separator();
                    foldoutState = GetOrCreateFoldoutState("Invalid Children : ", invalidField.ChildsValidations);
                    foldoutState.IsFoldOut = EditorGUILayout.Foldout(foldoutState.IsFoldOut, foldoutState.Name);
                    if (foldoutState.IsFoldOut)
                    {
                        EditorGUI.indentLevel++;
                        foreach (RecursiveAssetValidation invalidAsset in invalidField.ChildsValidations)
                        {
                            DisplayAssetValidation($"- {invalidAsset.AssetName}", invalidAsset);
                        }
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}