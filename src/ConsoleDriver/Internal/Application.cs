using System;
using System.Collections.Generic;
using System.Linq;

using Caliberweb.Core;

using ConsoleDriver.Internal.Config;

namespace ConsoleDriver.Internal
{
    class Application : IApplication, IDisposable
    {
        private readonly Queue<IConfig> configs;
        private readonly LogConfig logConfig;

        public Application()
        {
            ConsoleReader = new ConsoleReader();
            
            configs = new Queue<IConfig>();
            
            logConfig = new LogConfig();
            
            configs.Enqueue(logConfig);
        }

        public void AddConfiguration(IConfig config)
        {
            if (configs.Contains(config))
            {
                return;
            }
            configs.Enqueue(config);
        }

        public ConsoleReader ConsoleReader { get; private set; }

        public void OpenLastLog()
        {
            logConfig.OpenLastLog();
        }

        void IDisposable.Dispose()
        {
            while(configs.Count > 0)
            {
                configs.Dequeue().Shutdown(this);
            }
        }

        public void Configure()
        {
            configs.ToList().ForEach(c=>c.Configure(this));
        }
    }
}