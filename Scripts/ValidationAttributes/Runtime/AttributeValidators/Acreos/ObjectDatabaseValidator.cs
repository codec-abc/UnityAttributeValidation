using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DUDE.Core.Database;

namespace AttributeValidation
{
    public class ObjectDatabaseValidator : BaseValidator
    {
        public override bool Validate(object attributeFieldObj, object ownerObj, FieldInfo fieldInfo)
        {
#if UNITY_EDITOR
            var db = ObjectDatabase.GetOrCreateSettings();

            if (db == null)
            {
                throw new System.Exception($"Impossible to load the object database inside {nameof(ObjectDatabaseValidator)} for field {fieldInfo}");
            }

            var objs = db.Objects;

            if (attributeFieldObj == null)
            {
                return false;
            }
            if (attributeFieldObj is IEnumerable<string> fieldValues)
            {
                return
                    fieldValues
                    .Where(str => !string.IsNullOrWhiteSpace(str))
                    .All(obj => objs.Any(a => a.ObjectName == obj));
            }
            else if (attributeFieldObj is string fieldValue)
            {
                if (!string.IsNullOrWhiteSpace(fieldValue))
                {
                    return objs.Any(a => a.ObjectName == fieldValue);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                throw new System.Exception($"Invalid type for {nameof(DUDE.Core.Attributes.ObjectDatabaseAttribute)} for field {fieldInfo}");
            }
#else
        return true;
#endif
        }
    }
}