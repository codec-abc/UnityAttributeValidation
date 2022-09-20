using AttributeValidation;
using UnityEngine;
using static AttributeValidation.AssetKindValidator;

#pragma warning disable 0169
#pragma warning disable 0649
#pragma warning disable CA1823

public class TestAttributeValidator5 : MonoBehaviour
{
    [SerializeField, NotNull]
    [AssetKindValidation(AssetKind.AnyButSceneObject)]
    private UnityEngine.Object m_not_scene_obj1;

    [SerializeField, NotNull]
    [AssetKindValidation(AssetKind.AnyButSceneObject)]
    private UnityEngine.Object m_not_scene_obj2;

    [SerializeField, NotNull]
    [AssetKindValidation(AssetKind.SceneObject)]
    private UnityEngine.Object m_scene_obj1;

    [SerializeField, NotNull]
    [AssetKindValidation(AssetKind.SceneObject)]
    private UnityEngine.Object m_scene_obj2;
}

#pragma warning restore 0169
#pragma warning restore 0649
#pragma warning restore CA1823