using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace Doctr.Diagnostics
{

    public static class ObjectExtensions
    {

        public static string Dump(this object @object)
        {
            
            return JsonSerializer.Serialize(@object, 
                new JsonSerializerOptions() { WriteIndented = true });

        }

        public static string Internal(this object @object, string name)
        {
            var objectType = @object.GetType();
            var member = objectType.GetField(name, 
                    BindingFlags.NonPublic | BindingFlags.Instance) as MemberInfo ??
                objectType.GetProperty(name,
                    BindingFlags.NonPublic | BindingFlags.Instance) as MemberInfo;

            Trace.WriteLine(member == null);

            if(member != null) 
            {
                if(member.MemberType == MemberTypes.Field)
                {
                    return ObjectExtensions.Dump(((FieldInfo)member).GetValue(@object));
                }
                else if(member.MemberType == MemberTypes.Property)
                {
                    return ObjectExtensions.Dump(((PropertyInfo)member).GetValue(@object));
                }
            }

            return null;
        }

    }

}
