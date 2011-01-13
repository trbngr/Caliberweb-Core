using System;

namespace Caliberweb.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsConcrete<T>(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface && typeof (T).IsAssignableFrom(type);
        }
    }
}