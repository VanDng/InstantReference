using System;
using System.Collections.Generic;
using System.Text;

namespace InstanceReference
{
    class WebLookupResult : LookupResult
    {
        public string SourceName { get; private set; }
        public string Url { get; private set; }
        public string Html { get; private set; }

        public WebLookupResult(string sourceName, string url, string html)
        {
            SourceName = sourceName;
            Url = url;
            Html = html;
        }
    }
}
