using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstanceReference
{
    class CustomResourceRequestHandler : ResourceRequestHandler, IResourceRequestHandler
    {
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            Uri url;
            if (Uri.TryCreate(request.Url, UriKind.Absolute, out url) == false)
            {
                //If we're unable to parse the Uri then cancel the request
                // avoid throwing any exceptions here as we're being called by unmanaged code
                return CefReturnValue.Cancel;
            }
            
            if (AdBlockManager.IsBlock(url))
            {
                return CefReturnValue.Cancel;
            }

            return CefReturnValue.Continue;
        }
    }
}
