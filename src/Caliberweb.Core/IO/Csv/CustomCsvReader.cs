using System.Collections.Generic;
using System.Linq;

using Caliberweb.Core.Specification;

using OpenFileSystem.IO;

namespace Caliberweb.Core.IO.Csv
{
    public abstract class CustomCsvReader<T>
    {
        private readonly IFile file;
        private CsvReader reader;

        protected CustomCsvReader(IFile file)
        {
            this.file = file;
        }

        public abstract CsvDescription Description { get; }

        public IEnumerable<T> Records
        {
            get { return Query(Spec<T>.Empty); }
        }

        protected abstract T CreateRecord(ICsvRecord record);

        private CsvReader InitializeReader()
        {
            return new CsvReader(file, Description);
        }

        public IEnumerable<T> Query(ISpec<T> spec)
        {
            if (reader == null)
            {
                reader = InitializeReader();
            }

            IEnumerable<ICsvRecord> records = reader.GetRecords();

            return records.Select(r => CreateRecord(r)).Where(r => spec.IsSatisfied(r));
        }
    }
}