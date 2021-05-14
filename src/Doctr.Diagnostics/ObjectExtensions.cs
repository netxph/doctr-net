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

    }

}
