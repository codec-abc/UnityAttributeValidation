using UnityEngine;

namespace AttributeValidation
{
    [CreateAssetMenu]
    public class AttributeValidationScriptableConfig : ScriptableObject
    {
        [SerializeField]
        [AttributeValidation.NotNullAttribute]
        private AttributeValidationConfig m_config;

        public AttributeValidationConfig Config => m_config;
    }
}