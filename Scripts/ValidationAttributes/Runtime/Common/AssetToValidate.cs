using System.Collections.Generic;

namespace AttributeValidation
{
    public class AssetToValidate
    {
        public readonly object Asset;

        public readonly string AssetName = "";
        public readonly AssetToValidate Parent;
        public readonly string BasePath = "";

        public AssetToValidate(
            object asset,
            AssetToValidate parentAsset,
            string assetName,
            string basePath)
        {
            Asset = asset;
            Parent = parentAsset;
            AssetName = assetName;
            BasePath = basePath;
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

            return parentList;
        }
    }
}