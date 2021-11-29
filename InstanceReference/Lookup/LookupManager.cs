using InstanceReference.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstanceReference
{
    partial class LookupManager
    {
        public event LookupResultHandler OnLookupCompleted;

        public delegate void LookupResultHandler(LookupResult lookupResult);
    }

    partial class LookupManager : IDisposable
    {
        private List<ILookupWorker> _workers;

        public LookupManager()
        {
            _workers = new List<ILookupWorker>();
        }

        public void Dispose()
        {
            foreach (var worker in _workers)
            {
                var disposableWorker = worker as IDisposable;
                disposableWorker?.Dispose();
            }
        }

        public void Lookup(string text)
        {
            foreach(var worker in _workers)
            {
                worker.Lookup(text);
            }
        }

        public void Intialize(LookupSource[] lookupSources)
        {
            string configContent = File.ReadAllText(Global.Constant.LookupSourceFilePath);

            foreach (var source in lookupSources)
            {
                var webSource = source as WebLookupSource;
                if (webSource != null)
                {
                    var worker = new WebLookupWorker(webSource.Name, webSource.BaseAddress, webSource.Uri);

                    worker.OnLookupCompleted += (lookupResult) =>
                    {
                        OnLookupCompleted(new WebLookupResult(lookupResult.SourceName, lookupResult.Html));
                    };

                    _workers.Add(worker);
                }
            }
        }
    }
}
