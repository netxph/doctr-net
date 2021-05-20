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

        public static string Dump(this object @object, string name)
        {
            return ObjectExtensions.Dump(
                ObjectExtensions.Internal(
                    @object, name));
        }

        public static object Internal(this object @object, string name)
        {
            var objectType = @object.GetType();
            var member = objectType.GetField(name, 
                    BindingFlags.NonPublic | BindingFlags.Instance) as MemberInfo ??
                objectType.GetProperty(name,
                    BindingFlags.NonPublic | BindingFlags.Instance) as MemberInfo;

            if(member != null) 
            {
                if(member.MemberType == MemberTypes.Field)
                {
                    return ((FieldInfo)member).GetValue(@object);
                }
                else if(member.MemberType == MemberTypes.Property)
                {
                    return ((PropertyInfo)member).GetValue(@object);
                }
            }

            return null;
        }

        public static T Internal<T>(this object @object, string name)
        {
            return (T)ObjectExtensions.Internal(@object, name);
        }

    }

}
