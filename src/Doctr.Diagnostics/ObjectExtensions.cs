using System;
using System.Reflection;
using System.Text.Json;
using System.Linq;

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

        public static string ReflectDump(this Type type)
        {
            var internalFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var publicFlags = BindingFlags.Public | BindingFlags.Instance;

            var reflectInfo = new
            {
                AssemblyQualifiedName = type.AssemblyQualifiedName,
                Properties = type.GetProperties(publicFlags).Where(p => !p.IsSpecialName).Select(p => p.Name).ToList(),
                Fields = type.GetFields(publicFlags).Where(f => !f.IsSpecialName).Select(f => f.Name).ToList(),
                Methods = type.GetMethods(publicFlags).Where(m => !m.IsSpecialName).Select(m => m.Name).ToList(),
                Internals = new 
                {
                    Properties = type.GetProperties(internalFlags).Where(p => !p.IsSpecialName).Select(p => p.Name).ToList(),
                    Fields = type.GetFields(internalFlags).Where(f => !f.IsSpecialName).Select(f => f.Name).ToList(),
                    Methods = type.GetMethods(internalFlags).Where(m => !m.IsSpecialName).Select(m => m.Name).ToList(),
                }
            };

            return ObjectExtensions.Dump(reflectInfo);
        }

    }

}
