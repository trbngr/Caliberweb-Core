using System;
using System.Collections.Generic;

using Caliberweb.Core.Specification;

using NUnit.Framework;

using OpenFileSystem.IO.FileSystems.InMemory;

using System.Linq;

using Caliberweb.Core.Extensions;

namespace Caliberweb.Core.IO.Csv
{
    [TestFixture(new object[] { "file1.csv", 15 })]
    [TestFixture(new object[] { "file2.csv", 250 })]
    public class StronglyTypedCsvReading
    {
        private readonly string filename;
        private readonly int recordCount;
        private MyCustomCsvReader reader;

        public StronglyTypedCsvReading(string filename, int recordCount)
        {
            this.filename = filename;
            this.recordCount = recordCount;
        }

        [TestFixtureSetUp]
        public virtual void SetUp()
        {
            var file = new InMemoryFile(filename);

            reader = new MyCustomCsvReader(file);

            var lines = CreateFileContents(recordCount).ToArray();

            file.WriteLines(lines);
        }

        [Test]
        public void ThisIsHowToReadAllRecords()
        {
            foreach (var record in reader.Records)
            {
                Console.Out.WriteLine("{0:MM/dd/yyyy}: {1}", record.Date, record.Words);
            }
        }

        [Test]
        public void ThisIsHowToQueryTheFile()
        {
            var d = new DateTime(1950, 6, 1);

            var spec = Spec<CustomCsvRecord>
                .Create(r => r.Date > d)
                .And(r => r.Words.Length > 50)
                .AndNot(r => r.Words.Length > 100);

            Console.Out.WriteLine("Constraints....");
            Console.Out.WriteLine("Greater than {0}", d);
            Console.Out.WriteLine("Word length > 50");
            Console.Out.WriteLine("Word length <= 100");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("orderd by date");
            Console.Out.WriteLine("");

            var records = reader.Query(spec).OrderBy(r => r.Date);

            foreach (var record in records)
            {
                Console.Out.WriteLine("{0:MM/dd/yyyy}: [{2}] {1}", record.Date, record.Words, record.Words.Length);
            }
        }

        private static IEnumerable<string> CreateFileContents(int recordCount)
        {
            yield return "\"date\"\t\"number\"\t\"words\"";

            for (int i = 0; i < recordCount; i++)
            {
                yield return String.Format("\"{0}\"\t\"{1}\"\t\"{2}\"", Rand.NextDate(), Rand.Next(),
                                           Rand.String.NextSentence(5, 25));
            }
        }
    }
}