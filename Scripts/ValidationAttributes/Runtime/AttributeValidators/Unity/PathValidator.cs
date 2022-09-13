using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    public class PathValidator : BaseValidator
    {
        public enum PathForObjectKind
        {
            File,
            Folder,
            Any,
        }

        public enum PathKind
        {
            Absolute,
            Relative,
        }

        private readonly string m_startingPath;
        private readonly PathForObjectKind m_targetedObjectByPath;
        private readonly PathKind m_pathKind;
        private readonly List<string> m_allowedExtensions;

        public PathValidator(
            string startingPath,
            PathForObjectKind targetedObjectByPath,
            PathKind pathKind,
            List<string> allowedExtensions)
        {
            m_startingPath = startingPath.Replace("\\", "/");
            m_targetedObjectByPath = targetedObjectByPath;
            m_pathKind = pathKind;
            m_allowedExtensions = allowedExtensions;
        }

        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            if (attributeFieldObj == null)
            {
                return false;
            }

            if (attributeFieldObj.GetType() != typeof(string))
            {
                throw new Exception($"[{nameof(PathValidator)}] Cannot validate a path which is not stored as a string for {fieldInfo}");
            }

            var attributeAsString = (string)attributeFieldObj;
            attributeAsString = attributeAsString.Replace("\\", "/");

            var dataPath = Application.dataPath;
            dataPath = dataPath.Replace("\\", "/");

            var absolutePath = attributeAsString;

            if (m_pathKind == PathKind.Relative)
            {
                absolutePath = Path.Combine(dataPath, attributeAsString);
                absolutePath = absolutePath.Replace("\\", "/");
                if (IsPathFullyQualified(attributeAsString))
                {
                    return false;
                }
            }
            else if (m_pathKind == PathKind.Absolute)
            {
                if (!IsPathFullyQualified(attributeAsString))
                {
                    return false;
                }
            }

            if (!File.Exists(absolutePath) && !Directory.Exists(absolutePath))
            {
                return false;
            }

            var fileAttribute = File.GetAttributes(absolutePath);
            var isDir = fileAttribute.HasFlag(FileAttributes.Directory);

            if (isDir && m_targetedObjectByPath == PathForObjectKind.File)
            {
                return false;
            }

            if (!isDir && m_targetedObjectByPath == PathForObjectKind.Folder)
            {
                return false;
            }

            if (
                !string.IsNullOrWhiteSpace(m_startingPath) &&
                !attributeAsString.StartsWith(m_startingPath))
            {
                return false;
            }

            if (m_targetedObjectByPath == PathForObjectKind.File)
            {
                var extension = Path.GetExtension(attributeAsString);

                var isOk =
                    m_allowedExtensions.Contains(".") ||
                    m_allowedExtensions.Contains("*") ||
                    m_allowedExtensions.Any(ext => ext.ToLower() == extension.ToLower());

                return isOk;
            }
            else
            {
                return true;
            }
        }

        public static bool IsPathFullyQualified(string path)
        {
            if (
                string.IsNullOrWhiteSpace(path) ||
                path.IndexOfAny(Path.GetInvalidPathChars()) != -1 ||
                !Path.IsPathRooted(path))
            {
                return false;
            }

            string pathRoot = Path.GetPathRoot(path);

            // Accepts X:\ and \\UNC\PATH, rejects empty string, \ and X:, but accepts / to support Linux
            if (pathRoot.Length <= 2 && pathRoot != "/")
            {
                return false;
            }

            if (pathRoot[0] != '\\' || pathRoot[1] != '\\')
            {
                // Rooted and not a UNC path
                return true;
            }

            // A UNC server name without a share name (e.g "\\NAME" or "\\NAME\") is invalid
            return pathRoot.Trim('\\').IndexOf('\\') != -1;
        }
    }
}