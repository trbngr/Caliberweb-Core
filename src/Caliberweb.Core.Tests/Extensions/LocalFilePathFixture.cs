// ReSharper disable AssignNullToNotNullAttribute
using System;
using System.IO;
using System.Threading;

using NUnit.Framework;

namespace Caliberweb.Core.Extensions
{
    [TestFixture]
    public class LocalFilePathFixture
    {
        private const string FILENAME = "tests/file.txt";

        private string filePath;

        [SetUp]
        public void BeforeEachTest()
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FILENAME);
        }

        [TearDown]
        public void AfterEachTest()
        {
            string directoryName = Path.GetDirectoryName(filePath);

            Directory.Delete(directoryName);
        }

        [Test]
        public void WillCreateDirectory()
        {
            string path = FILENAME.ToLocalFilePath(true);

            Assert.AreEqual(filePath, path);

            Assert.IsTrue(Directory.Exists(Path.GetDirectoryName(path)));
        }

        [Test]
        public void WillNotThrowIfRequestToCreateDirectoryAndDirectoryExists()
        {
            Assert.DoesNotThrow(()=>
            {
                FILENAME.ToLocalFilePath(true);

                FILENAME.ToLocalFilePath(true);
            });
        }
    }
}
// ReSharper restore AssignNullToNotNullAttribute