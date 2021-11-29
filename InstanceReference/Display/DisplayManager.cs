using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace InstanceReference
{
    class DisplayManager
    {
        private TabControl _tabContainer;
        private Hashtable _sourceMaps;

        public DisplayManager(TabControl tabContainer)
        {
            _tabContainer = tabContainer;
            _sourceMaps = new Hashtable();
        }

        public void Initialize(LookupSource[] lookupSources)
        {
            _tabContainer.Items.Clear();

            foreach (var source in lookupSources)
            {
                var webSource = source as WebLookupSource;
                if (webSource != null)
                {
                    var tab = new TabItem();
                    tab.Header = webSource.Name;

                    var webControl = new WebBrowser();
                    tab.Content = webControl;

                    _tabContainer.Items.Add(tab);

                    _sourceMaps.Add(webSource.Name, webControl);
                }
            }
        }

        public void ShowLookupResult(LookupResult lookupResult)
        {
            var webLookup = lookupResult as WebLookupResult;

            var webControl = _sourceMaps[webLookup.SourceName] as WebBrowser;

            // Performance Issue !!
            //webControl.Dispatcher.Invoke(() =>
            //{
            //    webControl.Navigate("https://dictionary.cambridge.org/vi/dictionary/english/morning");
            //});
        }
    }
}
