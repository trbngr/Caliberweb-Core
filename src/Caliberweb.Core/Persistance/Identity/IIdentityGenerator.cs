namespace Caliberweb.Core.Persistance.Identity
{
    public interface IIdentityGenerator
    {
        object GetNext();
    }
}