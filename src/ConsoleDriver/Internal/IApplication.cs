using Caliberweb.Core;

using ConsoleDriver.Internal.Config;

namespace ConsoleDriver.Internal
{
    internal interface IApplication
    {
        ConsoleReader ConsoleReader { get; }
        void AddConfiguration(IConfig config);
        void OpenLastLog();
    }
}