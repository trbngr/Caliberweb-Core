using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Caliberweb.Core.Net.Scraping
{
    internal class ZipFileLinkParser : ILinkParser
    {
        private const string MATCHING_PATTERN = ".+.zip";
        private const RegexOptions OPTIONS = RegexOptions.IgnoreCase;

        private readonly Regex regex;

        public ZipFileLinkParser()
        {
            regex = new Regex(MATCHING_PATTERN, OPTIONS);
        }

        #region ILinkParser Members

        public IEnumerable<Uri> FindLinks(Uri source, string html)
        {
            Match results = regex.Match(html);
            while (results.Success)
            {
                string value = results.Groups[0].Value;

                if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                {
                    yield return new Uri(value);
                }

                if(Uri.IsWellFormedUriString(value, UriKind.Relative))
                {
                    yield return new Uri(source, value);
                }

                results = results.NextMatch();
            }
        }

        #endregion
    }
}