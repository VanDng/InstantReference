using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstanceReference
{
    static partial class Global
    {
        static public class Setup
        {
            static public void Install()
            {
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
                {
                    Converters = new List<Newtonsoft.Json.JsonConverter>() { new Newtonsoft.Json.Converters.StringEnumConverter() }
                };
            }
        }
    }
}
