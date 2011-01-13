using System;
using System.Diagnostics;
using System.IO;

namespace Caliberweb.Core.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static void CreateFresh(this DirectoryInfo directory)
        {
            if(directory.Exists)
                directory.Delete(true);
            directory.Create();
        }

        public static IDisposable Explore(this DirectoryInfo directory)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("explorer", directory.FullName)
            };

            process.Start();

            return new DisposableAction(()=>
            {
                process.Refresh();

                if(!process.HasExited)
                {
                    process.Kill();
                }
            });
        }
    }
}