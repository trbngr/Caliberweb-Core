using System;
using System.Collections.Generic;
using System.Linq;

using Caliberweb.Core.Extensions;
using Caliberweb.Core.Reflection;

namespace Caliberweb.Core.Serialization
{
    public static class KnownTypes
    {
        private static readonly Dictionary<Type, Func<Type, IEnumerable<Type>>> typeResolvers = new Dictionary<Type, Func<Type, IEnumerable<Type>>>();

        public static IEnumerable<Type> Of<T>(IAssemblyProvider assemblyProvider)
        {
            var type = typeof(T);

            if(!typeResolvers.ContainsKey(type))
            {
                var resolver = new Func<Type, IEnumerable<Type>>(t => assemblyProvider.Assemblies.SelectMany(a => a.GetExportedTypes().Where(t1 => t1.IsConcrete<T>()))).Memoize();
    
                typeResolvers.Add(type, resolver);
            }

            return typeResolvers[type](type);
        }
    }
}