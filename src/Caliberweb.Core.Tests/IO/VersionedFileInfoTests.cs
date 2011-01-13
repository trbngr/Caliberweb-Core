using System.IO;

using NUnit.Framework;

namespace Caliberweb.Core.IO
{
    [TestFixture]
    public class VersionedFileInfoTests
    {
        [Test]
        public void if_the_file_is_not_versioned_the_currentGen_is_0_and_the_next_gen_is_1()
        {
            const string ROOT_FILE = "original.file.txt";

            var version = new GenerationalFileBackupInfo(new FileInfo(ROOT_FILE));

            Assert.AreEqual(0, version.CurrentGeneration);
            Assert.AreEqual(1, version.NextGeneration);
        }

        [Test]
        public void if_the_file_is_versioned_the_currentGen_is_version_and_nextGen_is_incremented_by_1()
        {
            const string ROOT_FILE = "original.file.txt.1";

            var version = new GenerationalFileBackupInfo(new FileInfo(ROOT_FILE));

            Assert.AreEqual(1, version.CurrentGeneration);
            Assert.AreEqual(2, version.NextGeneration);
        }

        [Test]
        public void if_the_file_is_versioned_the_next_file_has_the_next_gen_appended_to_filename()
        {
            const string ROOT_FILE = "original.file.txt.1";

            var version = new GenerationalFileBackupInfo(new FileInfo(ROOT_FILE));

            Assert.AreEqual("original.file.txt.1", version.CurrentFile.Name);
            Assert.AreEqual("original.file.txt.002", version.NextFile.Name);
        }

        [Test]
        public void if_the_file_is_not_versioned_the_next_file_has_the_next_gen_appended_to_filename()
        {
            const string ROOT_FILE = "original.file.txt";

            var version = new GenerationalFileBackupInfo(new FileInfo(ROOT_FILE));

            Assert.AreEqual("original.file.txt", version.CurrentFile.Name);
            Assert.AreEqual("original.file.txt.001", version.NextFile.Name);
        }
    }
}