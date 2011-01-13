using System;

namespace Caliberweb.Core.Persistance.Identity
{
    class GuidIdentityGenerator : IIdentityGenerator
    {
        public Guid GetNext()
        {
            return Guid.NewGuid();
        }

        object IIdentityGenerator.GetNext()
        {
            return GetNext();
        }
    }
}