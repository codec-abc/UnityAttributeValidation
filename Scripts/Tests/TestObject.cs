using System;
using System.Collections.Generic;
using AttributeValidation;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    public int Int = default;
    public int? Intnull = null;

    [AttributeValidation.NotNull]
    public string String = default;

    public bool Bool = default;
    [MinValue(12)]
    public float Float = default;
    [MinValue(12)]
    public double Double = default;
    [NotNull]
    public List<string> ListStrings = default;

    [EnumerableNotNull, EnumerableMinValue(15), Min(14)]
    public List<int?> ListInts = new List<int?>();

    [NotNull]
    public TestObject TestObj = default;

    [NotNull]
    public Transform Transf = default;

    [NotNull, EnumerableNotNull]
    public List<TestObject> ListObjects = default;
}

[Serializable]
public struct TestStruct
{
}