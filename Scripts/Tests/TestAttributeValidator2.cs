using System.Reflection;
using AttributeValidation;
using UnityEngine;

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

    private static bool IsEven(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
    {
        if (attributeFieldObj is int intValue)
        {
            return intValue % 2 == 0;
        }

        return false;
    }
}
