using System;

using OpenFileSystem.IO;

namespace Caliberweb.Core.IO.Csv
{
    public class MyCustomCsvReader : CustomCsvReader<CustomCsvRecord>
    {
        private readonly IColumn<DateTime> dateColumn;
        private readonly IColumn<int> numberColumn;
        private readonly IColumn<string> wordsColumn;
        private readonly CsvDescription description;

        public MyCustomCsvReader(IFile file) : base(file)
        {
            dateColumn = Columns.Date("date");
            numberColumn = Columns.Integer("number");
            wordsColumn = Columns.String("words");

            description = new CsvDescription(new IColumn[]
            {
                dateColumn,
                numberColumn,
                wordsColumn
            });
        }

        protected override CustomCsvRecord CreateRecord(ICsvRecord record)
        {
            var values = record.Values;

            return new CustomCsvRecord
            {
                Date = values.GetValue(dateColumn),
                Number = values.GetValue(numberColumn),
                Words = values.GetValue(wordsColumn)
            };
        }

        public override CsvDescription Description
        {
            get { return description; }
        }
    }
}