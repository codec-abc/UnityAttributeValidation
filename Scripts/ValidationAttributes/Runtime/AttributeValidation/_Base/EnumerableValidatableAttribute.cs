using System.Collections;
using System.Reflection;

namespace AttributeValidation
{
    public abstract class EnumerableValidatableAttribute : BaseValidatableAttribute
    {
        public override bool Validate(object obj, object parentObj, FieldInfo fieldInfo, IValidationContext validationContext)
        {
            var validator = GetValidator();

            // TODO: handle dictionary?
            if (typeof(IEnumerable).IsAssignableFrom(obj.GetType()))
            {
                foreach (object enuObj in (IEnumerable)obj)
                {
                    if (validationContext.ShouldIgnoreEnumerableObj(enuObj, obj, parentObj, fieldInfo))
                    {
                        continue;
                    }

                    if (!validator.Validate(enuObj, parentObj, fieldInfo))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                var msg = $"Cannot apply an EnumerableValidatableAttribute to a variable of type {obj.GetType()}";
                throw new System.Exception(msg);
            }
        }
    }
}