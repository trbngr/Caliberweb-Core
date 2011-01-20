using System.Collections.Generic;

using NUnit.Framework;

using System.Linq;

namespace Caliberweb.Core.IO.Csv
{
    [TestFixture("test1.csv")]
    [TestFixture("test2.csv")]
    [TestFixture("test3.csv")]
    [TestFixture("test4.csv")]
    [TestFixture("test1.csv")]
    [TestFixture("test2.csv")]
    [TestFixture("test3.csv")]
    [TestFixture("test4.csv")]
    public class CsvReaderTests
    {
        #region Setup/Teardown

        [SetUp]
        public virtual void SetUp()
        {
            if(!files.ContainsKey(filename))
            {
                files.Add(filename, file = new TestMemoryFile(filename, 100));
                Assert.That(file.ReadCount, Is.EqualTo(0));
            }

            file = files[filename];

            reader = new CsvReader(file, TestMemoryFile.Description);
        }

        #endregion

        private TestMemoryFile file;
        private readonly string filename;
        private CsvReader reader;
        private static readonly IDictionary<string, TestMemoryFile> files = new Dictionary<string, TestMemoryFile>();

        public CsvReaderTests(string filename)
        {
            this.filename = filename;
        }

        [Test]
        public void AFileIsOnlyReadFromDiskOnce()
        {
            var array = reader.GetRecords().ToArray();
            
            Assert.That(file.ReadCount, Is.EqualTo(1));
            Assert.That(array.Length, Is.EqualTo(100));

            array = reader.GetRecords().ToArray();

            Assert.That(file.ReadCount, Is.EqualTo(1));
            Assert.That(array.Length, Is.EqualTo(100));
        }
    }
}