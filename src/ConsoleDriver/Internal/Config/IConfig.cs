namespace ConsoleDriver.Internal.Config
{
    interface IConfig
    {
        void Configure(IApplication application);
        void Shutdown(IApplication application);
    }
}