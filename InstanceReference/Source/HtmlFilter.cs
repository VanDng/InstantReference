using System;
using System.Collections.Generic;
using System.Text;

namespace InstanceReference
{
    class HtmlFilter
    {
        public string[] Selectors { get; private set; }

        public HtmlFilter(string[] selector)
        {
            Selectors = selector;
        }
    }
}
