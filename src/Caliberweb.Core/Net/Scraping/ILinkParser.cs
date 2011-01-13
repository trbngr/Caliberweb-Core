using System;
using System.Collections.Generic;

namespace Caliberweb.Core.Net.Scraping
{
    public interface ILinkParser
    {
        IEnumerable<Uri> FindLinks(Uri source, string html);
    }
}