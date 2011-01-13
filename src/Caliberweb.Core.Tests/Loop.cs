using System;

namespace Caliberweb.Core
{
    public static class Loop
    {
        public static void For(TimeSpan timeSpan)
        {
            DateTime start = DateTime.Now;
            while (DateTime.Now - start < timeSpan)
            {}
        }
    }
}