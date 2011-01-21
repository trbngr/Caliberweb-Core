using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Caliberweb.Core;
using Caliberweb.Core.Specification;
using Caliberweb.Core.Verizon;

using ConsoleDriver.Internal;

using log4net;

using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystems.Local;

using Path = System.IO.Path;

using System.Linq;

using Caliberweb.Core.Extensions;

namespace ConsoleDriver
{
    internal class Runner : IRunner
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (Runner));
        private readonly LocalFileSystem fs;
        private readonly ConsoleReader reader;

        public Runner(ConsoleReader reader)
        {
            this.reader = reader;
            fs = LocalFileSystem.Instance;
        }
        
        #region IRunner Members

        public void Run()
        {
            var backupDirectory = fs.GetDirectory(@"D:\!saved");

            var dt = new DateTime(2010, 9, 3);
            var dirSpec = Spec<IDirectory>.Create(d => d.LastWriteTimeUtc() > dt);

            var fileSpec = Spec<IFile>.Create(f => f.LastModifiedTimeUtc > dt)
                .And(f => f.Extension == ".mp3");

            var files = fs.GetDirectory(@"D:\!music")
                .Directories()
                .Where(dirSpec.IsSatisfied)
                .SelectMany(d => d.Files("*", SearchScope.SubFolders)
                                     .Where(fileSpec.IsSatisfied))
                .ToList();

            foreach (var file in files)
            {
                var pathQueue = new Queue<string>(file.Path.Segments);

                pathQueue.Dequeue();
                pathQueue.Dequeue();

                var remainingPath = pathQueue.Aggregate("", (p, c) => string.Concat(p, c, "/")).TrimEnd('/');

                var newPath = Path.Combine(backupDirectory.Path.FullPath, remainingPath);

                var dest = fs.GetFile(newPath);
                
                file.CopyTo(dest);
            }
            //            const string PATH = @"C:\Users\cmartin.NTSPHX1\Downloads\phonecalls";
//
//            var minutes = new VerizonMinutesReader(new[]
//            {
//                fs.GetFile(Path.Combine(PATH, "6023265200.csv")),
//                fs.GetFile(Path.Combine(PATH, "6025096418.csv")),
//                fs.GetFile(Path.Combine(PATH, "6025096464.csv")),
//                fs.GetFile(Path.Combine(PATH, "6025097062.csv")),
//            });
//
//            ISpec<VerizonRecord> spec = VerizonRecord.NumberIsNot("Misty")
//                .And(VerizonRecord.NotTollFree)
//                .And(VerizonRecord.MinutesGreaterThan(9));
//
//            foreach (VerizonRecord record in minutes.GetFriendsAndFamilyRecommendations(spec))
//            {
//                log.Info(record);
//            }
//
//            foreach (VerizonRecord record in minutes.GroupByNumber())
//            {
//                log.Info(record);
//            }
        }

        #endregion
    }
}