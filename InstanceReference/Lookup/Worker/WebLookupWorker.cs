using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InstanceReference
{
    partial class WebLookupWorker
    {
        public event LookupResultHandler OnLookupCompleted;

        public delegate void LookupResultHandler(WebLookupResult clipBoardText);
    }

    partial class WebLookupWorker : BaseLookupWorker, ILookupWorker, IDisposable
    {
        private HttpClient _httpClient;
        private WebLookupSource _lookupSource;

        public WebLookupWorker(WebLookupSource lookupSource)
        {
            _lookupSource = lookupSource;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(lookupSource.BaseAddress);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task Lookup(string text)
        {
            var lookupResult = await WebQuery(text);
            var url = $"{_httpClient.BaseAddress}{_lookupSource.Uri}{HttpUtility.HtmlEncode(text)}";

            OnLookupCompleted(new WebLookupResult(_lookupSource.Name, url, lookupResult));
        }

        private async Task<string> WebQuery(string text)
        {
            string encodedText = HttpUtility.HtmlEncode(text);
            string uri = $"{_lookupSource.Uri}{encodedText}";

            var httpResonse = await _httpClient.GetAsync(uri);

            if (httpResonse.Content != null)
            {
                var oriContent = await httpResonse.Content.ReadAsStringAsync();
                var newContent = ResponseFilter(_httpClient.BaseAddress.ToString(), oriContent);
                return newContent;
            }
            else
            {
                return "Something went wrong.";
            }
        }

        private string ResponseFilter(string url, string responseContent)
        {
            if (_lookupSource.HtmlFilter == null)
            {
                return responseContent;
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(responseContent);

            var uri = new Uri(url);
            var host = uri.Host;

            foreach (var selector in _lookupSource.HtmlFilter.Selectors)
            {
                if (string.IsNullOrEmpty(selector))
                {
                    continue;
                }

                var targetNodes = doc.DocumentNode.SelectNodes(selector);
                if (targetNodes != null)
                {
                    foreach (var target in targetNodes)
                    {
                        target.Remove();
                    }
                }
            }

            var t = "//script";
            foreach (var node in doc.DocumentNode.SelectNodes(t))
            {
                // It intends to remove ads generation scripts
                // but it's not really good approach, actually.
                // Just because Ads Blocking implementation has some issues now haha
                if (node.Attributes.Contains("src") == false)
                {
                    node.Remove();
                }
                else
                {
                    var scriptUrl = node.Attributes["src"].Value;
                    Uri tmpUri;
                    if (Uri.TryCreate(scriptUrl, UriKind.Absolute, out tmpUri))
                    {
                        if (AdBlockManager.IsBlock(tmpUri))
                        {
                            node.Remove();
                        }
                    }   
                }
            }

            return doc.DocumentNode.OuterHtml;
        }
    }
}
