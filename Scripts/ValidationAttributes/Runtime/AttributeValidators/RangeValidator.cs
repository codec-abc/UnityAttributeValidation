using System.Reflection;
using UnityEngine;

namespace AttributeValidation
{
    public class RangeValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
            var value = GetValue(attributeFieldObj);
            var attribute = (RangeAttribute)fieldInfo.GetCustomAttribute(typeof(RangeAttribute));
            return value >= attribute.min && value <= attribute.max;
        }

        private static double GetValue(object attributeFieldObj)
        {
            if (attributeFieldObj is byte a)
            {
                return a;
            }
            if (attributeFieldObj is sbyte b)
            {
                return b;
            }
            if (attributeFieldObj is char c)
            {
                return c;
            }
            if (attributeFieldObj is double d)
            {
                return d;
            }
            if (attributeFieldObj is float e)
            {
                return e;
            }
            if (attributeFieldObj is int f)
            {
                return f;
            }
            if (attributeFieldObj is uint g)
            {
                return g;
            }
            if (attributeFieldObj is long h)
            {
                return h;
            }
            if (attributeFieldObj is ulong i)
            {
                return i;
            }
            if (attributeFieldObj is short j)
            {
                return j;
            }
            if (attributeFieldObj is ushort k)
            {
                return k;
            }
            if (attributeFieldObj is UnityEngine.Rendering.PostProcessing.FloatParameter floatParam)
            {
                return floatParam.value;
            }
            if (attributeFieldObj is UnityEngine.Rendering.PostProcessing.IntParameter intParam)
            {
                return intParam.value;
            }

            throw new System.Exception($"type for range attribute is not valid {attributeFieldObj.GetType()}");
            //if (attributeFieldObj is decimal d)
            //{
            //    return d;
            //}
            //if (attributeFieldObj is nint a)
            //{
            //    return a;
            //}
            //if (attributeFieldObj is nuint a)
            //{
            //    return a;
            //}
        }
    }
}
