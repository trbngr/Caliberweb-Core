using System;

using Caliberweb.Core.Caching.Storage;

namespace Caliberweb.Core.Caching
{
    public class TestLocalDataEventHandler : ILocalDataEventHandler
    {
        public void OnInit(IStorageStrategy sender)
        {
            StrategyUsed = sender;
            Console.Out.WriteLine("initialized {0}", sender);
        }

        public void OnValueAdded<T>(IStorageStrategy sender, object key, T value)
        {
            StrategyUsed = sender;
            KeyUsed = key;
            ValueUsed = value;

            Console.Out.WriteLine("added value to {0}[{1}]", sender, key);
        }

        public void OnValueNotFound(IStorageStrategy sender, object key)
        {
            StrategyUsed = sender;
            KeyUsed = key;
            Console.Out.WriteLine("value not found in {0}[{1}]", sender, key);
        }

        public void OnValueFound<T>(IStorageStrategy sender, object key, T value)
        {
            StrategyUsed = sender;
            KeyUsed = key;
            ValueUsed = value;
            Console.Out.WriteLine("value found in {0}[{1}]", sender, key);
        }

        public void OnValueRemoved(IStorageStrategy sender, object key)
        {
            StrategyUsed = sender;
            KeyUsed = key;
            Console.Out.WriteLine("value removed from {0}[{1}]", sender, key);
        }

        public void OnClear(IStorageStrategy sender)
        {
            StrategyUsed = sender;
            Console.Out.WriteLine("cleared {0}", sender);
        }

        public IStorageStrategy StrategyUsed { get; private set; }

        public object KeyUsed { get; private set; }

        public object ValueUsed { get; private set; }
    }
}