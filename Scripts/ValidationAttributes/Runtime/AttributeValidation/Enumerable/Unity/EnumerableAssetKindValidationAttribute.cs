using System;
using AttributeValidation;
using static AttributeValidation.AssetKindValidator;

namespace EnumerableAttributeValidation
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumerableAssetKindValidationAttribute : EnumerableValidatableAttribute
    {
        private readonly AssetKind m_assetKind;

        public EnumerableAssetKindValidationAttribute(AssetKind assetKind)
        {
            m_assetKind = assetKind;
        }

        protected override BaseValidator GetValidator()
        {
            return new AssetKindValidator(m_assetKind);
        }
    }
}