using DistillNET;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace InstanceReference
{
    static class AdBlockManager
    {
        static private FilterDbCollection _filterCollection;
        static private bool _isReady = false;

        static AdBlockManager()
        {
            _filterCollection = new FilterDbCollection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rules.db"), true, true);

            var files = Directory.GetFiles(@$"{AppDomain.CurrentDomain.BaseDirectory}\AdBlock", "*.data");

            for(int idx = 0; idx < files.Length; idx++)
            {
                var fileStream = File.OpenRead(files[idx]);
                
                var result = _filterCollection.ParseStoreRulesFromStream(fileStream, (short)(idx + 1));

                Debug.WriteLine($"OK: {result.Item1}, NG: {result.Item2}");

                // Not so good.. hmm. it's still better than nothing.. though.
                // Dive deeper when have time.

                // OK: 29542, NG: 26898
                // OK: 26703, NG: 1
                // OK: 7765, NG: 41933

                fileStream?.Dispose();
            }

            // Ensure that we build the index AFTER we're all done our inserts.
            _filterCollection.FinalizeForRead();

            Debug.WriteLine($"AdBlock Loaded Count: {_filterCollection.GetFiltersForDomain().Count()}");

            _isReady = true;

        }

        static public bool IsBlock(Uri uri)
        {
            if (_isReady == false)
            {
                return false;
            }

            bool isBlock = false;

            var domain = uri.Host;
            var rules = _filterCollection.GetFiltersForDomain(domain);

            var headers = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

            foreach (var rule in rules)
            {
                if (rule.IsMatch(uri, headers))
                {
                    isBlock = true;
                    break;
                }
            }

            return isBlock;
        }
    }
}
