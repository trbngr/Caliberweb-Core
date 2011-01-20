using System;
using System.Collections;
using System.IO;

using Caliberweb.Core.Extensions;

using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystems.InMemory;

using Path = OpenFileSystem.IO.Path;

namespace Caliberweb.Core.IO.Csv
{
    internal class TestMemoryFile : IFile
    {
        private readonly int lines;

        public static readonly CsvDescription Description = new CsvDescription(new IColumn[]
        {
            Columns.Integer("id"),
            Columns.String("name")
        });

        private readonly InMemoryFile wrapped;
        private int readCount;

        public TestMemoryFile(string filePath, int lines)
        {
            this.lines = lines;
            wrapped = new InMemoryFile(filePath);
            WriteContent();
        }

        #region IFile Members

        public IFile Create()
        {
            return wrapped.Create();
        }

        public void Delete()
        {
            wrapped.Delete();
        }

        public void CopyTo(IFileSystemItem item)
        {
            wrapped.CopyTo(item);
        }

        public void MoveTo(IFileSystemItem item)
        {
            wrapped.MoveTo(item);
        }

        public Path Path
        {
            get { return wrapped.Path; }
        }

        public IDirectory Parent
        {
            get { return wrapped.Parent; }
        }

        public IFileSystem FileSystem
        {
            get { return wrapped.FileSystem; }
        }

        public bool Exists
        {
            get { return true; }
        }

        public string Name
        {
            get { return wrapped.Name; }
        }

        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            readCount++;
            return wrapped.Open(fileMode, fileAccess, fileShare);
        }

        public string NameWithoutExtension
        {
            get { return wrapped.NameWithoutExtension; }
        }

        public string Extension
        {
            get { return wrapped.Extension; }
        }

        public long Size
        {
            get { return wrapped.Size; }
        }

        public DateTime? LastModifiedTimeUtc
        {
            get { return wrapped.LastModifiedTimeUtc; }
        }

        #endregion

        private void WriteContent()
        {
            var records = new ArrayList
            {
                "\"id\"\t\"name\""
            };

            for (int i = 0; i < lines; i++)
            {
                records.Add(String.Format("\"{0}\"\t\"{1}\"", i, Rand.String.NextWord()));
            }

            wrapped.WriteLines((string[]) records.ToArray(typeof(string)));
        }

        public int ReadCount
        {
            get { return readCount; }
        }
    }
}