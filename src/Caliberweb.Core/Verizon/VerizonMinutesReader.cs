using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Caliberweb.Core.IO;
using Caliberweb.Core.Specification;

namespace Caliberweb.Core.Verizon
{
    public class VerizonMinutesReader
    {
        private readonly IColumn<DateTime> dateColumn;
        private readonly IColumn<string> descriptionColumn;
        private readonly IColumn<int> minutesColumn;
        private readonly IColumn<string> numberColumn;
        private readonly List<CsvReader> readers;

        public VerizonMinutesReader(IEnumerable<FileInfo> files)
        {
            minutesColumn = Columns.Integer("Minutes");
            numberColumn = Columns.String("Number");
            dateColumn = Columns.Date("Date");
            descriptionColumn = Columns.String("Desc");

            var description = new CsvDescription(new IColumn[]
            {
                minutesColumn,
                numberColumn,
                dateColumn,
                descriptionColumn
            });

            readers = new List<CsvReader>();

            foreach (var file in files)
            {
                readers.Add(new CsvReader(file, description));
            }
        }

        public IEnumerable<VerizonRecord> GetFriendsAndFamilyRecommendations(ISpec<VerizonRecord> spec)
        {
            return Records
                .Where(spec.IsSatisfied)
                .GroupBy(r => r.Number)
                .Select(g => new VerizonRecord
                {
                    Date = g.OrderBy(r => r.Date).Last().Date,
                    Description = g.First().Description,
                    Minutes = g.Sum(r => r.Minutes),
                    Number = g.Key
                }).Take(10).OrderByDescending(r => r.Minutes);
        }

        public IEnumerable<VerizonRecord> Records
        {
            get
            {
                var vr = new List<VerizonRecord>();

                foreach (var reader in readers)
                {
                    var records = reader.GetRecords();

                    vr.AddRange(records.Select(r => new VerizonRecord
                    {
                        Date = r.Values.GetColumnValue(dateColumn),
                        Description = r.Values.GetColumnValue(descriptionColumn),
                        Minutes = r.Values.GetColumnValue(minutesColumn),
                        Number = r.Values.GetColumnValue(numberColumn)
                    }));
                }

                return vr;
            }
        }
    }
}