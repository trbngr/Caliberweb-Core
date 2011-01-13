using System.IO;

using NUnit.Framework;

namespace Caliberweb.Core.IO
{
    [TestFixture]
    public class InMemoryFileBackupTests
    {
        private DirectoryInfo tempDirectory;
        private string text;
        private FileInfo file;

        [SetUp]
        public virtual void SetUp()
        {
            tempDirectory = new DirectoryInfo("./tests");
            tempDirectory.Create();

            text = Rand.String.NextSentence(10, 500);

            const string FILENAME = "original_file_name.txt";
            file = new FileInfo(Path.Combine(tempDirectory.FullName, FILENAME));
            
            File.WriteAllText(file.FullName, text);
        }

        [TearDown]
        public virtual void TearDown()
        {
            tempDirectory.Delete(true);
        }

        [Test]
        public void can_alter_and_restore_file_with_using_statement()
        {
            var path = file.FullName;

            using(new InMemoryFileBackup(file))
            {
                File.AppendAllText(path, Rand.String.NextSentence(5, 35));

                var actual = ReadText();

                Assert.AreNotEqual(text, actual);
            }
            
            Assert.AreEqual(text, ReadText());
        }

        [Test]
        public void can_alter_and_restore_file()
        {
            var path = file.FullName;
            
            //create backup
            var backup = new InMemoryFileBackup(file);

            //alter file
            File.AppendAllText(path, Rand.String.NextSentence(5, 35));
            var actual = ReadText();
            Assert.AreNotEqual(text, actual);
            
            //restore file
            backup.Dispose();
            Assert.AreEqual(text, ReadText());
        }

        private string ReadText()
        {
            return File.ReadAllText(file.FullName);
        }
    }
}