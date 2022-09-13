using System;
using static AttributeValidation.AssetKindValidator;

namespace AttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AssetKindValidationAttribute : SingleElementValidatableAttribute
    {
        private readonly AssetKind m_assetKind;

        public AssetKindValidationAttribute(AssetKind assetKind)
        {
            m_assetKind = assetKind;
        }

        protected override BaseValidator GetValidator()
        {
            return new AssetKindValidator(m_assetKind);
        }
    }
}