using System;
using System.Collections.Generic;

namespace Caliberweb.Core.Net.Scraping
{
    public class ZipFileScraper : IResourceScraper
    {
        private readonly IWebClient client;
        private readonly ILinkParser parser;

        public ZipFileScraper(IWebClient client)
        {
            this.client = client;
            parser = new AllLinkParser(new ZipFileLinkParser());
        }

        public IEnumerable<Uri> Scrape(Uri address)
        {
            string html = client.Download(address);

            return parser.FindLinks(address, html);
        }
    }
}