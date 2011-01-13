using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;

namespace ConsoleDriver.Internal.Config
{
    internal class LogConfig : IConfig
    {
        #region IConfig Members

        public void Configure(IApplication application)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");

            XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
        }

        public void Shutdown(IApplication application)
        {
            Console.Out.WriteLine("");

            if (application.ConsoleReader.Confirm("View log?"))
            {
                application.OpenLastLog();
            }

            LogManager.Shutdown();
        }

        #endregion

        public void OpenLastLog()
        {
            ILoggerRepository repository = LogManager.GetRepository();

            IAppender appender = repository.GetAppenders().Where(a => a is FileAppender).SingleOrDefault();

            new Process
            {
                StartInfo = new ProcessStartInfo("notepad", ((FileAppender) appender).File)
            }.Start();
        }
    }
}