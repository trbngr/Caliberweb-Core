namespace Caliberweb.Core
{
    public interface IObjectCloner
    {
        T Clone<T>(T instance);
    }
}