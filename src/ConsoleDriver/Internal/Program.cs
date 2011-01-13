using System;

using Caliberweb.Core.Benchmarking;

using ConsoleDriver.Internal.Config;

using log4net;

namespace ConsoleDriver.Internal
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (Program));

        static void Main()
        {
            using (var application = new Application())
            {
                LogExceptions(() =>
                {
                    application.AddConfiguration(new EndOfProgramConfig());

                    Configure(application);

                    Run(new Runner(application.ConsoleReader));
                });
            }
        }

        private static void Run(IRunner runner)
        {
            Action a = runner.Run;
            log.InfoFormat("runtime: {0}.", a.Time());
        }

        private static void Configure(Application application)
        {
            Action a = application.Configure;
            log.InfoFormat("configured in: {0}.", a.Time());
        }

        internal static void LogExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }
    }
}
