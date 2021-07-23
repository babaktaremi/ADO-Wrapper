using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Type = System.Type;

namespace AdoWrapper.Extensions
{
   internal static class ObjectExtensions
    {
        internal static Dictionary<string, Type> GetProperties<T>(this T obj) 
        {
            var result = new Dictionary<string, Type>();

            var properties = obj.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                result.Add(propertyInfo.Name, propertyInfo.PropertyType);
            }

            return result;
        }

        internal static void FillProperty<T>(this T obj, string propertyName, object value)
        {
            obj.GetType().InvokeMember(propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, obj, new[] {value});
        }
        
    }
}
