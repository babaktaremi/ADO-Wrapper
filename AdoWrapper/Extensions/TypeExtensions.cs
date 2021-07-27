using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
    }
}