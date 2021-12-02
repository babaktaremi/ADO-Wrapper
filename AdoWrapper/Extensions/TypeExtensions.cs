using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdoWrapper.Markers;

namespace AdoWrapper.Extensions
{
    internal static class TypeExtensions
    {
        internal static readonly ConcurrentDictionary<Type, List<PropertyInfo>> TypeProperties = new();

        internal static List<PropertyInfo> GetWritableProperties<T>()
        {
            var type = typeof(T);

            return TypeProperties.GetOrAdd(type, _ =>
            {
                return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    //Only writable properties
                    .Where(p => p.GetSetMethod() is not null).ToList();
            });
        }

        internal static List<PropertyInfo> GetWritableProperties(this object obj)
        {
            var type = obj.GetType();

            return TypeProperties.GetOrAdd(type, _ =>
            {
                return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    //Only writable properties
                    .Where(p => p.GetSetMethod() is not null).ToList();
            });
        }


        internal static bool IsClassProperty(this PropertyInfo property)
        {
            if (!property.CanWrite)
                throw new ArgumentException($"The property is not writable {property.Name}");

            return property.PropertyType.IsClass && property.GetCustomAttributes(typeof(ForeignNavigation), false).Count() != 0;
        }

        internal static bool IsPropertyGenericList(this PropertyInfo property)
        {
            return property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition()
                == typeof(List<>);
        }

        internal static Type GetGenericArgument(this PropertyInfo property) => property.PropertyType.GetGenericArguments()[0];

        internal static List<IList> GetListProperties<T>()
        {
            var properties = GetWritableProperties<T>().Where(c=>c.IsPropertyGenericList()).ToList();

            var result = properties.Select(propertyInfo => Activator.CreateInstance(propertyInfo.PropertyType) as IList).ToList();
            return result;
        }
    }
}