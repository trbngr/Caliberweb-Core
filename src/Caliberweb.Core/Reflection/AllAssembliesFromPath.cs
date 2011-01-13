using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Caliberweb.Core.Reflection
{
    internal class AllAssembliesFromPath : IAssemblyProvider
    {
        private readonly Predicate<Assembly> filter;
        private readonly string path;

        public AllAssembliesFromPath(string path, Predicate<Assembly> filter)
        {
            this.path = path;
            this.filter = filter;
        }

        #region IAssemblyProvider Members

        public IEnumerable<Assembly> Assemblies
        {
            get
            {
                // ReSharper disable PossibleNullReferenceException
                const StringComparison COMPARISON = StringComparison.OrdinalIgnoreCase;

                IEnumerable<string> eligibleFiles = Directory.GetFiles(path)
                    .Where(file => Path.GetExtension(file).Equals(".exe", COMPARISON) ||
                                   Path.GetExtension(file).Equals(".dll", COMPARISON)).ToArray();

                IEnumerable<Assembly> assemblies = eligibleFiles
                    .Select(file => Assembly.LoadFrom(file))
                    .Where(a => filter(a));

                return assemblies;

                // ReSharper restore PossibleNullReferenceException
            }
        }

        #endregion
    }
}