using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Caliberweb.Core.Net.Scraping
{
    class AllLinkParser : ILinkParser
    {
        private readonly ILinkParser parser;
        private const string PATTERN = @"<a href=""([\w-.://_]+)"">(.*?)</a>";
        private const RegexOptions OPTIONS = RegexOptions.Singleline | RegexOptions.IgnoreCase;
        private readonly Regex regex;

        public AllLinkParser(ILinkParser parser)
        {
            this.parser = parser;
            regex = new Regex(PATTERN, OPTIONS);
        }

        public IEnumerable<Uri> FindLinks(Uri source, string html)
        {
            IEnumerable<string> uris = FindAllLinks(html);
            
            html = string.Join(Environment.NewLine, uris.ToArray());
            
            return parser.FindLinks(source, html).ToList();
        }

        private IEnumerable<string> FindAllLinks(string html)
        {
            var results = regex.Match(html);
            while (results.Success)
            {
                yield return results.Groups[1].Value;
                results = results.NextMatch();
            }
        }
    }
}