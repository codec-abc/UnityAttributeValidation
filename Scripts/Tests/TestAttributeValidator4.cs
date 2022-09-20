using AttributeValidation;
using UnityEngine;
using static AttributeValidation.RequireComponentValidator;

#pragma warning disable 0169
#pragma warning disable 0649
#pragma warning disable CA1823

public class TestAttributeValidator4 : MonoBehaviour
{
    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.Self)]
    private GameObject m_comp_1_self_good;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.Self)]
    private Transform m_comp_1_self_bad;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.ChildsAndSelf)]
    private GameObject m_comp_2_child_good;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.ChildsAndSelf)]
    private Transform m_comp_2_child_bad;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.ParentsAndSelf)]
    private GameObject m_comp_3_parent_good;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.ParentsAndSelf)]
    private Transform m_comp_3_parent_bad;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.ChildsAndSelf, 2)]
    private GameObject m_comp_4_multiples_good;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.ChildsAndSelf, 2)]
    private Transform m_comp_4_multiples_bad;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.ChildsAndSelf, SearchOption.IncludeInactive)]
    private GameObject m_comp_5_inactive_good;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.ChildsAndSelf, SearchOption.ExcludeInactive)]
    private Transform m_comp_6_active_good;

    [SerializeField, NotNull, RequireComponents(typeof(BoxCollider), HierarchyPosition.ChildsAndSelf, SearchOption.ExcludeInactive)]
    private Transform m_comp_6_active_bad;
}

#pragma warning restore 0169
#pragma warning restore 0649
#pragma warning restore CA1823