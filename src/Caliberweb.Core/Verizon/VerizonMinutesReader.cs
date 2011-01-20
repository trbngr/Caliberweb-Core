using System;
using System.Collections.Generic;
using System.Linq;
using Caliberweb.Core.IO.Csv;
using Caliberweb.Core.Specification;

using OpenFileSystem.IO;

namespace Caliberweb.Core.Verizon
{
    public class VerizonMinutesReader
    {
        private readonly IColumn<DateTime> dateColumn;
        private readonly IColumn<string> descriptionColumn;
        private readonly IColumn<int> minutesColumn;
        private readonly IColumn<string> numberColumn;
        private readonly List<CsvReader> readers;
        private readonly IColumn<DateTime> timeColumn;

        public VerizonMinutesReader(IEnumerable<IFile> files)
        {
            minutesColumn = Columns.Integer("Minutes");
            numberColumn = Columns.String("Number");
            dateColumn = Columns.Date("Date");
            timeColumn = Columns.Date("Time");
            descriptionColumn = Columns.String("Desc");

            var description = new CsvDescription('\t', new IColumn[]
            {
                minutesColumn,
                numberColumn,
                timeColumn,
                dateColumn,
                descriptionColumn
            });

            readers = files.Select(f => new CsvReader(f, description)).ToList();
        }

        public IEnumerable<VerizonRecord> Records
        {
            get
            {
                IEnumerable<ICsvRecord> allRecords = readers.SelectMany(r => r.GetRecords());

                return allRecords.Select(r =>
                {
                    DateTime date = r.Values.GetValue(dateColumn);
                    TimeSpan time = r.Values.GetValue(timeColumn).TimeOfDay;

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
            return GetFriendsAndFamilyRecommendations(Spec<VerizonRecord>.Empty);
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

        public IEnumerable<VerizonRecord> GroupByNumber()
        {
            return GroupByNumber(Spec<VerizonRecord>.Empty);
        }

        public IEnumerable<VerizonRecord> GroupByNumber(ISpec<VerizonRecord> spec)
        {
            return Records
                .Where(spec.IsSatisfied)
                .GroupBy(r => r.Number)
                .Select(g => CreateRecordFromGrouping(g))
                .OrderByDescending(r => r.Minutes);
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
    }
}