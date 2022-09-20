using AttributeValidation;
using UnityEngine;

#pragma warning disable 0169
#pragma warning disable 0649
#pragma warning disable CA1823

public class TestAttributeValidator6 : MonoBehaviour
{
    [SerializeField, NotNull, ResourceValidation]
    private string m_resourcePath_good1;

    [SerializeField, NotNull, ResourceValidation]
    private string m_resourcePath_bad1;

    [SerializeField, NotNull, AddressableValidation]
    private string m_addressable_good2;

    [SerializeField, NotNull, AddressableValidation]
    private string m_addressable_bad2;
}

#pragma warning restore 0169
#pragma warning restore 0649
#pragma warning restore CA1823