using System;
using System.Collections.Generic;
using System.Linq;

namespace AttributeValidation
{
    public class AssetToValidate
    {
        public readonly object Asset;

        public readonly string AssetName = "";
        public readonly AssetToValidate Parent;
        public readonly string BasePath = "";
        public readonly string Hierarchy = "/";

        public AssetToValidate(object asset, AssetToValidate parentAsset)
        {
            Asset = asset;
            Parent = parentAsset;
            try
            {
                AssetName = Asset.ToString();
            }
            catch (Exception)
            {
            }
        }

        public AssetToValidate(object asset, string basePath, string initialHierarchy)
        {
            Asset = asset;
            BasePath = basePath;
            Hierarchy = initialHierarchy;
            try
            {
                AssetName = Asset.ToString();
            }
            catch (Exception)
            {
            }
        }

        public override bool Equals(object obj)
        {
            return obj is AssetToValidate other && other.Asset == Asset;
        }

        public override int GetHashCode()
        {
            try
            {
                return Asset.GetHashCode();
            }
            catch
            {
                return 0;
            }
        }

        public string GetAssetNameSafe()
        {
            return AssetName;
        }

        public string GetFullAssetPath()
        {
            List<AssetToValidate> parentList = GetRootAssetToCurrent();
            var msg = parentList[0].BasePath + ":";
            msg += parentList.Aggregate("", (a, b) => a + "/" + b.GetAssetNameSafe());
            return msg;
        }

        public string GetHierarchy()
        {
            List<AssetToValidate> parentList = GetRootAssetToCurrent();
            var msg = "";
            foreach (var elem in parentList)
            {
                msg += elem.Hierarchy;
            }

            return msg;
        }

        public string GetTopParentBasePath()
        {
            List<AssetToValidate> parentList = GetRootAssetToCurrent();
            var msg = parentList[0].BasePath;
            return msg;
        }

        private List<AssetToValidate> GetRootAssetToCurrent()
        {
            var currentAsset = this;
            var parentList = new List<AssetToValidate>();
            while (currentAsset.Parent != null)
            {
                parentList.Add(currentAsset);
                currentAsset = currentAsset.Parent;
            }

            parentList.Add(currentAsset);
            parentList.Reverse();

            // foreach (var elem in parentList)
            // {
            //    debugMsg += $"Name: {elem.AssetName}, Hierarchy: {elem.Hierarchy}, BasePath: {elem.BasePath} " + "\n";
            // }
            // Debug.LogError($"debugMsg is {debugMsg}");
            return parentList;
        }
    }
}