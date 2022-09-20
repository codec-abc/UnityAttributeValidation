using AttributeValidation;
using DUDE.Core.Attributes;
using UnityEngine;

#pragma warning disable 0169
#pragma warning disable 0649
#pragma warning disable CA1823

public class TestAttributeValidator7 : MonoBehaviour
{
    [SerializeField, NotNull]
    private ObjectReference m_objectReference_good;

    [SerializeField, NotNull]
    private ObjectReference m_objectReference_bad;
}

#pragma warning restore 0169
#pragma warning restore 0649
#pragma warning restore CA1823