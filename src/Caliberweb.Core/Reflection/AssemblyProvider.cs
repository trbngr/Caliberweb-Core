using System;
using System.Reflection;

namespace Caliberweb.Core.Reflection
{
    public static class AssemblyProvider
    {
        public static IAssemblyProvider FromType<T>()
        {
            return new TypeLocationAssemblyProvider<T>();
        }

        public static IAssemblyProvider FromPath(string path)
        {
            return new AllAssembliesFromPath(path, Filters.nullFilter);
        }

        public static IAssemblyProvider FromPath(string path, Predicate<Assembly> filter)
        {
            return new AllAssembliesFromPath(path, filter);
        }

        public static IAssemblyProvider FromApplicationBase()
        {
            return new AllAssembliesFromPath(AppDomain.CurrentDomain.BaseDirectory, Filters.nullFilter);
        }

        public static IAssemblyProvider FromApplicationBase(Predicate<Assembly> filter)
        {
            return new AllAssembliesFromPath(AppDomain.CurrentDomain.BaseDirectory, filter);
        }

        public static class Filters
        {
            internal static readonly Predicate<Assembly> nullFilter = a => true;

            public static readonly Predicate<Assembly> NonVendor = a =>
            {
                const StringComparison COMPARISON = StringComparison.OrdinalIgnoreCase;
                if (a.FullName.StartsWith("mscorlib", COMPARISON))
                    return false;
                if (a.FullName.StartsWith("system", COMPARISON))
                    return false;
                if (a.FullName.StartsWith("microsoft", COMPARISON))
                    return false;
                return true;
            };
        }
    }
}