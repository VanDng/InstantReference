using System;
using System.Collections.Generic;
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
        private string _sourceName;
        private HttpClient _httpClient;
        private string _lookUri;

        public WebLookupWorker(string sourceName, string baseAddress, string lookupUri)
        {
            _sourceName = sourceName;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);

            _lookUri = lookupUri;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task Lookup(string text)
        {
            var lookupResult = await WebQuery(text);

            OnLookupCompleted(new WebLookupResult(_sourceName, lookupResult));
        }

        private async Task<string> WebQuery(string text)
        {
            string encodedText = HttpUtility.HtmlEncode(text);
            string uri = $"{_lookUri}{encodedText}";

            var httpResonse = await _httpClient.GetAsync(uri);
            
            if (httpResonse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await httpResonse.Content.ReadAsStringAsync();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
