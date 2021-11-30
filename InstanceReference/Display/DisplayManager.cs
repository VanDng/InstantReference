using CefSharp.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace InstanceReference
{
    class DisplayManager : IDisposable
    {
        private TabControl _tabContainer;
        private Hashtable _sourceMaps;

        public DisplayManager(TabControl tabContainer)
        {
            _tabContainer = tabContainer;
            _sourceMaps = new Hashtable();
        }

        public void Dispose()
        {
            foreach(var control in _sourceMaps.Values)
            {
                var webControl = control as ChromiumWebBrowser;
                if (webControl != null)
                {
                    webControl.Dispose();
                }
            }
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


                    var webControl = InitializeChroniumBrowserInstance();
                    tab.Content = webControl;

                    _tabContainer.Items.Add(tab);

                    _sourceMaps.Add(webSource.Name, webControl);
                }
            }
        }

        public void ShowLookupResult(LookupResult lookupResult)
        {
            var webLookup = lookupResult as WebLookupResult;
            if (webLookup != null)
            {
                var webControl = _sourceMaps[webLookup.SourceName] as ChromiumWebBrowser;

                webControl.Dispatcher.Invoke(() =>
                {
                    _ = CefSharp.WebBrowserExtensions.LoadHtml(webControl, webLookup.Html, webLookup.Url);
                    Debug.WriteLine(webLookup.Url);
                });
            }
        }

        private ChromiumWebBrowser InitializeChroniumBrowserInstance()
        {
            var webControl = new ChromiumWebBrowser();

            //webControl.BrowserSettings.ImageLoading = CefSharp.CefState.Disabled;
            //webControl.BrowserSettings.Javascript = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.Plugins = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.WebGl = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.WindowlessFrameRate = 10;
            webControl.BrowserSettings.Databases = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.ImageShrinkStandaloneToFit = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.JavascriptAccessClipboard = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.JavascriptCloseWindows = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.JavascriptDomPaste = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.LocalStorage = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.TabToLinks = CefSharp.CefState.Disabled;
            webControl.BrowserSettings.TextAreaResize = CefSharp.CefState.Disabled;

            return webControl;
        }
    }
}
