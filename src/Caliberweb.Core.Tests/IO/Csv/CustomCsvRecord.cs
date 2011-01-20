using System;

namespace Caliberweb.Core.IO.Csv
{
    public class CustomCsvRecord
    {
        public DateTime Date { get; internal set; }
        public int Number { get; internal set; }
        public string Words { get; internal set; }
    }
}