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
                var webSource = new WebLookupSource(source.name.ToString(),
                                                    source.base_address.ToString(),
                                                    source.uri.ToString());

                lookupSources.Add(webSource);
            }

            Sources = lookupSources.ToArray();
        }
    }
}