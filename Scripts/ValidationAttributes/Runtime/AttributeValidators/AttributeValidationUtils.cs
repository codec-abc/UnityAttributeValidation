namespace AttributeValidation
{
    public class AttributeValidationUtils
    {
        public static bool TryCastToFloat(object obj, out float floatVal)
        {
            bool objIsNumber;

            switch (obj)
            {
                case int intValue:
                    floatVal = intValue;
                    objIsNumber = true;
                    break;

                case float floatValue:
                    floatVal = floatValue;
                    objIsNumber = true;
                    break;

                case double doubleValue:
                    floatVal = (float)doubleValue;
                    objIsNumber = true;
                    break;

                case decimal decimalValue:
                    floatVal = (float)decimalValue;
                    objIsNumber = true;
                    break;

                case sbyte sbyteValue:
                    floatVal = sbyteValue;
                    objIsNumber = true;
                    break;

                case byte byteValue:
                    floatVal = byteValue;
                    objIsNumber = true;
                    break;

                case short shortValue:
                    floatVal = shortValue;
                    objIsNumber = true;
                    break;

                case ushort ushortValue:
                    floatVal = ushortValue;
                    objIsNumber = true;
                    break;

                case uint uintValue:
                    floatVal = uintValue;
                    objIsNumber = true;
                    break;

                case long longValue:
                    floatVal = longValue;
                    objIsNumber = true;
                    break;

                case ulong ulongValue:
                    floatVal = ulongValue;
                    objIsNumber = true;
                    break;
                default:
                    floatVal = default;
                    objIsNumber = false;
                    break;
            }

            return objIsNumber;
        }
    }
}