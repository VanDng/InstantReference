using InstanceReference.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstanceReference
{
    class SourceManager
    {
        public LookupSource[] Sources { get; private set; }

        public void Initialize(string configFilePath)
        {
            var configContent = File.ReadAllText(configFilePath);

            var lookupSources = new List<LookupSource>();

            dynamic sources = JsonConvert.DeserializeObject(configContent);
            foreach (var source in sources.sources)
            {
                HtmlFilter htmlFilter = null;

                if (source.ContainsKey("html_filter"))
                {
                    var selectors = source.html_filter.selectors.ToObject<string[]>();
                    htmlFilter = new HtmlFilter(selectors);
                }

                var webSource = new WebLookupSource(
                                                    source.name.ToString(),
                                                    source.base_address.ToString(),
                                                    source.uri.ToString(),
                                                    htmlFilter
                                                    );

                lookupSources.Add(webSource);
            }

            Sources = lookupSources.ToArray();
        }
    }
}