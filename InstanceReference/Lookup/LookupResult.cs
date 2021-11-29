using System;
using System.Collections.Generic;
using System.Text;

namespace InstanceReference
{
    abstract class LookupResult
    {
    }

    class WebLookupResult : LookupResult
    {
        public string SourceName { get; private set; }
        public string Html { get; private set; }

        public WebLookupResult(string sourceName, string html)
        {
            SourceName = sourceName;
            Html = html;
        }
    }
}
