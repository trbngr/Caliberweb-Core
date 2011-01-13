using System.Collections.Generic;
using System.Reflection;

namespace Caliberweb.Core.Reflection
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> Assemblies { get; }
    }
}