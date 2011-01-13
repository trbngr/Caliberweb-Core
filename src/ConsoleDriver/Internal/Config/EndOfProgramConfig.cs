using System;

namespace ConsoleDriver.Internal.Config
{
    class EndOfProgramConfig : IConfig
    {
        public void Configure(IApplication application)
        {}

        public void Shutdown(IApplication application)
        {
            Console.Out.WriteLine("press any key to exit.");
            Console.ReadKey(true);
        }
    }
}