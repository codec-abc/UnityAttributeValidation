using System;
using System.Collections.Generic;
using UnityEngine;

namespace AttributeValidation
{
    [Serializable]
    public class AttributeValidationConfig
    {
        public const string SCENE_FILE_EXTENSION = ".unity";
        public const string SPACE_INDENT = "  |  ";

        [SerializeField]
        private List<string> m_folderPathsToIgnore = new List<string>();

        [SerializeField]
        private List<string> m_fileExtensionsToIgnore = new List<string>();

        [SerializeField]
        private bool m_analyzeScenes = true;

        public AttributeValidationConfig(
            List<string> folderPathsToIgnore,
            List<string> fileExtensionsToIgnore,
            bool analyzeScenes)
        {
            m_folderPathsToIgnore = folderPathsToIgnore;
            m_fileExtensionsToIgnore = fileExtensionsToIgnore;
            m_analyzeScenes = analyzeScenes;
        }

        public IReadOnlyList<string> FolderPathsToIgnore => m_folderPathsToIgnore;

        public IReadOnlyList<string> FileExtensionsToIgnore => m_fileExtensionsToIgnore;

        public bool AnalyzeScenes => m_analyzeScenes;
    }
}