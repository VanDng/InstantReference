using System;
using System.Collections.Generic;
using System.Text;

namespace InstanceReference
{
    class WebLookupSource : LookupSource
    {
        public string Name { get; private set; }
        public string BaseAddress { get; private set; }
        public string Uri { get; private set; }

        public WebLookupSource(string name, string baseAddress, string uri)
        {
            Name = name;
            BaseAddress = baseAddress;
            Uri = uri;
        }
    }
}
