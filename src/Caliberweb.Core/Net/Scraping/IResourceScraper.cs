using System;
using System.Collections.Generic;

namespace Caliberweb.Core.Net.Scraping
{
    public interface IResourceScraper
    {
        IEnumerable<Uri> Scrape(Uri address);
    }
}