using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

namespace Caliberweb.Core.IO
{
    [TestFixture]
    public class RollingFileBackupTests
    {
        #region Setup/Teardown

        [SetUp]
        public virtual void SetUp()
        {
            tempDirectory = new DirectoryInfo("./backup-tests");
            tempDirectory.Create();
        }

        [TearDown]
        public virtual void TearDown()
        {
            tempDirectory.Delete(true);
        }

        #endregion

        private const string FILENAME = "original_file_name.txt";
        private const string SEARCH_PATTERN = "original_file_name.*";

        private DirectoryInfo tempDirectory;

        private static readonly IsContractExceptionConstraint isContractExceptionConstraint =
            new IsContractExceptionConstraint();

        private FileInfo CreateFile()
        {
            string path = GetFilePath();
            var fileInfo = new FileInfo(path);
            using (fileInfo.Create())
            {}
            File.SetLastWriteTime(path, DateTime.Now.AddHours(-6));
            return fileInfo;
        }

        private void CreateFiles(int number)
        {
            FileInfo info = CreateFile();

            const string RENAME_PATTERN = "{0}.{1:000}";

            DateTime current = DateTime.Now.AddHours(-6);

            for (int i = 0; i < number; i++)
            {
                string destFileName = string.Format(RENAME_PATTERN, info.FullName, i + 1);

                //In the case of a failing test, the files could still be on disk.
                if (File.Exists(destFileName))
                {
                    File.Delete(destFileName);
                }

                File.Copy(info.FullName, destFileName);

                current += TimeSpan.FromMinutes(15);

                File.SetLastWriteTime(destFileName, current);
            }
        }

        private string GetFilePath()
        {
            return Path.Combine(tempDirectory.FullName, FILENAME);
        }

        private List<FileInfo> GetFiles()
        {
            return Directory.GetFiles(tempDirectory.FullName, SEARCH_PATTERN)
                .OrderBy(s => s)
                .Select(f => new FileInfo(f))
                .ToList();
        }

        [Test]
        public void calling_backup_with_null_file_throws()
        {
            Assert.Throws(isContractExceptionConstraint, () => new RollingFileBackup(null, 1));
        }

        [Test]
        public void if_at_max_backups_only_keep_newest()
        {
            FileInfo file = CreateFile();

            var backup = new RollingFileBackup(file, 5);

            IEnumerable<IFileBackupInfo> versions = new GenerationalFileBackupInfo[0];

            for (int i = 0; i < 10; i++)
            {
                versions = backup.Create();
            }

            List<FileInfo> files = GetFiles();

            //original + 5 backups
            Assert.AreEqual(6, files.Count);
            Assert.AreEqual(5, versions.Count());
        }

        [Test]
        public void if_original_file_exists_create_backup_number_one()
        {
            FileInfo fileInfo = CreateFile();

            IFileBackup backup = new RollingFileBackup(fileInfo, 5);

            backup.Create();

            List<FileInfo> files = GetFiles();

            Assert.AreEqual(2, files.Count);
        }

        [Test]
        public void if_there_is_currently_more_than_maxBackups_the_rest_are_deleted()
        {
            CreateFiles(10);

            Assert.AreEqual(11, GetFiles().Count);

            var backup = new RollingFileBackup(new FileInfo(GetFilePath()), 5);

            IEnumerable<IFileBackupInfo> versions = backup.Create();

            Assert.AreEqual(5, versions.Count());
        }

        [Test]
        public void if_there_is_one_backup_move_number_one_to_number_two_and_create_number_one()
        {
            CreateFiles(1);

            string file = GetFilePath();

            var backup = new RollingFileBackup(new FileInfo(file), 5);
            backup.Create();

            List<FileInfo> files = GetFiles();

            Assert.AreEqual(3, files.Count);
        }

        [Test]
        public void initializing_with_maxBackup_of_zero_throws()
        {
            Assert.Throws(isContractExceptionConstraint, () => new RollingFileBackup(new FileInfo(".\\file.txt"), 0));
        }

        [Test]
        public void initializing_with_non_existant_file_throws()
        {
            Assert.Throws<FileNotFoundException>(() => new RollingFileBackup(new FileInfo(".\\file.txt"), 2));
        }
    }
}