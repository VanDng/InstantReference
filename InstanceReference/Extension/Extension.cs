using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace InstanceReference.Extension
{
    public static class ExpandoObjectHelper
    {
        // Reference:
        // // https://stackoverflow.com/questions/2839598/how-to-detect-if-a-property-exists-on-an-expandoobject
        public static bool HasProperty(ExpandoObject obj, string propertyName)
        {
            return obj != null && ((IDictionary<String, object>)obj).ContainsKey(propertyName);
        }
    }
}
