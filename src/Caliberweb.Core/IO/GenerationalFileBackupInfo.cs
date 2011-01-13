using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Caliberweb.Core.IO
{
    public class GenerationalFileBackupInfo : FileBackupInfoBase
    {
        private readonly Regex matcher;

        public GenerationalFileBackupInfo(FileInfo original) : base(original)
        {
            string name = Path.GetFileNameWithoutExtension(original.Name);
            matcher = new Regex(String.Format(@"{0}.(\d+)", name));

            SetGenerations();
            SetNextFile();
        }

        private void SetGenerations()
        {
            CurrentGeneration = 0;
            NextGeneration = 1;
            IsCurrentlyVersioned = false;

            string filename = CurrentFile.FullName;

            if (matcher.IsMatch(filename))
            {
                Match match = matcher.Match(filename);

                CurrentGeneration = int.Parse(match.Groups[1].Value);

                NextGeneration = CurrentGeneration + 1;
                IsCurrentlyVersioned = true;
            }
        }

        private void SetNextFile()
        {
            string name = (IsCurrentlyVersioned)
                              ? Path.GetFileNameWithoutExtension(CurrentFile.Name)
                              : CurrentFile.Name;

            string nextGenName = string.Format("{0}.{1:000}", name, NextGeneration);

            NextFile = new FileInfo(Path.Combine(CurrentFile.DirectoryName, nextGenName));
        }

        public int CurrentGeneration { get; private set; }
        public int NextGeneration { get; private set; }
    }
}