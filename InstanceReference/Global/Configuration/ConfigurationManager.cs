using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstanceReference
{
    class Configuration
    {
        public DictionaryWindowMode DictionaryWindow_WindowMode { get; set; }
        public WindowDimension DictionaryWindow_Demension { get; set; } = new WindowDimension();

        public TriggerWindowMode TriggerWindow_WindowMode { get; set; }
        public WindowDimension TriggerWindow_Dimension { get; set; } = new WindowDimension();

        public ShortcutCollection Shortcuts { get; private set; } = new ShortcutCollection();
    }

    static partial class Global
    {
        static public class ConfigurationManager
        {
            public static Configuration Configuration { get; private set; } = new Configuration();

            public static void Load(string configurationFilePath)
            {
                var configJson = File.ReadAllText(configurationFilePath);
                Configuration = JsonConvert.DeserializeObject<Configuration>(configJson);
            }

            public static void Save(string configurationFilePath)
            {
                string json = JsonConvert.SerializeObject(Configuration, Formatting.Indented);
                File.WriteAllText(configurationFilePath, json);
            }
        }
    }
}
