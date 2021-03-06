using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstanceReference
{
    class CustomRequestHandler : RequestHandler, IRequestHandler
    {
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            return new CustomResourceRequestHandler();
        }
    }
}
