using DUDE.Core.Exercise;

namespace AttributeValidation
{
    public class ObjectValidationFilters
    {
        public static bool IsUnityObjImageParameter(object obj)
        {
            if (obj is Parameter param)
            {
                var name = param.name;
                if (name.EndsWith("(image)"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}