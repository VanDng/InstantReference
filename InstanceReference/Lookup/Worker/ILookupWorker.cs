using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstanceReference
{
    interface ILookupWorker
    {
        public Task Lookup(string text);
    }
}
