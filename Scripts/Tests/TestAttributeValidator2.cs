using System.Reflection;
using AttributeValidation;
using DUDE.Core.Attributes;
using UnityEngine;

#pragma warning disable 0169
#pragma warning disable 0649
#pragma warning disable CA1823

public class TestAttributeValidator2 : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private string m_test1;

    [SerializeField]
    [NotNull]
    [StringNotEmpty]
    private string m_test2_good = " ";

    [SerializeField]
    [NotNull]
    [StringNotEmpty]
    private string m_test2_bad = "";

    [SerializeField]
    [NotNull]
    [StringNotWhiteSpace]
    private string m_test3_good = "a";

    [SerializeField]
    [NotNull]
    [StringNotWhiteSpace]
    private string m_test3_bad = " ";

    [SerializeField]
    [NotNull]
    [StringMatchRegex(@"\d+\.\d+\.\d+\.\d+")]
    private string m_test4_good = "192.168.1.1";

    [SerializeField]
    [NotNull]
    [StringMatchRegex(@"\d+\.\d+\.\d+\.\d+")]
    private string m_test4_bad = "bad ip";

    [SerializeField]
    [NotDefault]
    private Vector3 m_test5_good = new Vector3(1, 2, 1);

    [SerializeField]
    [NotDefault]
    private Vector3 m_test5_bad;

    [SerializeField]
    [CustomFunctionValidator(typeof(TestAttributeValidator2), nameof(IsEven))]
    private int m_test6_good = 42;

    [SerializeField]
    [CustomFunctionValidator(typeof(TestAttributeValidator2), nameof(IsEven))]
    private int m_test6_bad = 41;

    [SerializeField]
    [Range(-1, 1)]
    private float m_test7_good = 0.0f;

    [SerializeField]
    [Range(-1, 1)]
    private float m_test7_bad = 2.0f;

    [SerializeField]
    [Range(-1, 1)]
    private int m_test8_good = 0;

    [SerializeField]
    [Range(-1, 1)]
    private int m_test8_bad = 2;

    [SerializeField, NotNull, ObjectDatabase]
    protected string m_test9_good = string.Empty;

    [SerializeField, NotNull, ObjectDatabase]
    protected string m_test9_bad = string.Empty;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.JustSelf)]
    private GameObject m_test10_self_good;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.JustSelf)]
    private GameObject m_test10_self_bad;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ParentsOnly)]
    private GameObject m_test11_parent_only_good;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ParentsOnly)]
    private GameObject m_test11_parent_only_bad_0;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ParentsOnly)]
    private GameObject m_test11_parent_only_bad_1;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ParentsOnly)]
    private GameObject m_test11_parent_only_bad_2;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsOnly)]
    private GameObject m_test12_childs_only_good;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsOnly)]
    private GameObject m_test12_childs_only_bad_0;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsOnly)]
    private GameObject m_test12_childs_only_bad_1;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsOnly)]
    private GameObject m_test12_childs_only_bad_2;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ParentsAndSelf)]
    private GameObject m_test13_parent_self_good_0;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ParentsAndSelf)]
    private GameObject m_test13_parent_self_good_1;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ParentsAndSelf)]
    private GameObject m_test13_parent_self_bad_0;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ParentsAndSelf)]
    private GameObject m_test13_parent_self_bad_1;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsAndSelf)]
    private GameObject m_test14_child_self_good_0;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsAndSelf)]
    private GameObject m_test14_child_self_good_1;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsAndSelf)]
    private GameObject m_test14_child_self_bad_0;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsAndSelf)]
    private GameObject m_test14_child_self_bad_1;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsAndSelf)]
    private Transform m_test15_child_self_transform_good_0;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsAndSelf)]
    private Transform m_test15_child_self_transform_good_1;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsAndSelf)]
    private Transform m_test15_child_self_transform_bad_0;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.ChildsAndSelf)]
    private Transform m_test15_child_self_transform_bad_1;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.Any)]
    private GameObject m_test16_any_good_0;

    [SerializeField, NotNull, HierarchyValidation(HierarchyValidator.HierarchyPosition.Any)]
    private GameObject m_test16_any_good_1;

    private static bool IsEven(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
    {
        if (attributeFieldObj is int intValue)
        {
            return intValue % 2 == 0;
        }

        return false;
    }
}

#pragma warning restore 0169
#pragma warning restore 0649
#pragma warning restore CA1823