using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Caliberweb.Core.IO.Csv;
using Caliberweb.Core.Specification;

namespace Caliberweb.Core.Verizon
{
    public class VerizonMinutesReader
    {
        private readonly IColumn<DateTime> dateColumn;
        private readonly IColumn<DateTime> timeColumn;
        private readonly IColumn<string> descriptionColumn;
        private readonly IColumn<int> minutesColumn;
        private readonly IColumn<string> numberColumn;
        private readonly List<CsvReader> readers;

        public VerizonMinutesReader(IEnumerable<FileInfo> files)
        {
            minutesColumn = Columns.Integer("Minutes");
            numberColumn = Columns.String("Number");
            dateColumn = Columns.Date("Date");
            timeColumn = Columns.Date("Time");
            descriptionColumn = Columns.String("Desc");

            var description = new CsvDescription(new IColumn[]
            {
                minutesColumn,
                numberColumn,
                timeColumn,
                dateColumn,
                descriptionColumn
            });

            readers = new List<CsvReader>();

            foreach (var file in files)
            {
                readers.Add(new CsvReader(file, description));
            }
        }

        public IEnumerable<VerizonRecord> Records
        {
            get
            {
                var allRecords = readers.SelectMany(r => r.GetRecords());

                return allRecords.Select(r =>
                {
                    var date = r.Values.GetValue(dateColumn);
                    var time = r.Values.GetValue(timeColumn).TimeOfDay;

                    return new VerizonRecord
                    {
                        Date = date.Add(time),
                        Description = r.Values.GetValue(descriptionColumn),
                        Minutes = r.Values.GetValue(minutesColumn),
                        Number = r.Values.GetValue(numberColumn)
                    };
                });
            }
        }

        public IEnumerable<VerizonRecord> GetFriendsAndFamilyRecommendations()
        {
            return GetFriendsAndFamilyRecommendations(Spec.Empty<VerizonRecord>());
        }

        public IEnumerable<VerizonRecord> GetFriendsAndFamilyRecommendations(ISpec<VerizonRecord> spec)
        {
            return Records
                .GroupBy(r => r.Number)
                .Select(g => CreateRecordFromGrouping(g))
                .Where(spec.IsSatisfied)
                .OrderByDescending(r => r.Minutes)
                .Take(10);

        }

        private static VerizonRecord CreateRecordFromGrouping(IGrouping<string, VerizonRecord> g)
        {
            return new VerizonRecord
            {
                Date = g.OrderBy(r => r.Date).Last().Date,
                Description = g.First().Description,
                Minutes = g.Sum(r => r.Minutes),
                Number = g.Key
            };
        }

        public IEnumerable<VerizonRecord> GroupByNumber()
        {
            return GroupByNumber(Spec.Empty<VerizonRecord>());
        }

        public IEnumerable<VerizonRecord> GroupByNumber(ISpec<VerizonRecord> spec)
        {
            return Records
                .Where(spec.IsSatisfied)
                .GroupBy(r => r.Number)
                .Select(g => CreateRecordFromGrouping(g))
                .OrderByDescending(r => r.Minutes);
        }
    }
}