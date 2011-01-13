using System.Collections.Generic;
using System.Reflection;

namespace Caliberweb.Core.Reflection
{
    class TypeLocationAssemblyProvider<T> : IAssemblyProvider
    {
        public IEnumerable<Assembly> Assemblies
        {
            get { yield return typeof(T).Assembly; }
        }
    }
}