using System;
using System.IO;

using Caliberweb.Core;
using Caliberweb.Core.Verizon;

using ConsoleDriver.Internal;

using log4net;

using System.Linq;

namespace ConsoleDriver
{
    internal class Runner : IRunner
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (Runner));
        private readonly ConsoleReader reader;

        public Runner(ConsoleReader reader)
        {
            this.reader = reader;
        }

        #region IRunner Members

        public void Run()
        {
            const string PATH = @"C:\Users\cmartin.NTSPHX1\Downloads\phonecalls";

            var minutes = new VerizonMinutesReader(new[]
            {
                new FileInfo(Path.Combine(PATH, "6023265200.csv")),
                new FileInfo(Path.Combine(PATH, "6025096418.csv")),
                new FileInfo(Path.Combine(PATH, "6025096464.csv")),
                new FileInfo(Path.Combine(PATH, "6025097062.csv")),
            });

            var spec = VerizonRecord.NumberIsNot("Misty")
                .And(VerizonRecord.NotTollFree)
                .And(VerizonRecord.MinutesGreaterThan(9));

            foreach (var record in minutes.GetFriendsAndFamilyRecommendations(spec))
            {
                log.Info(record);
            }

//            foreach (var record in minutes.GroupByNumber())
//            {
//                log.Info(record);
//            }
        }

        #endregion


    }

}