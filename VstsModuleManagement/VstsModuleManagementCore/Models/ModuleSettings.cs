namespace VstsModuleManagementCore.Models
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    using VstsModuleManagementCore.Utilities;
    using VstsModuleManagementCore.Utilities.JsonConverters;

    [JsonObject]
    public class ModuleSettings
    {
        public ModuleSettings()
        {
            this.KnownVstsProviders = new Dictionary<string, string>();
        }

        [JsonProperty]
        public bool HasCompleatedFirstRun { get; set; }

        [JsonProperty(ItemConverterType = typeof(DictionaryStringStringConverter))]
        [JsonConverter(typeof(DictionaryStringStringConverter))]
        public Dictionary<string,string> KnownVstsProviders { get; set; }

        internal void SaveSettings(string filePath)
        {
            string serializedSettings = JsonConvert.SerializeObject(this);

            File.WriteAllText(filePath, serializedSettings);
        }

        internal static ModuleSettings LoadSettings()
        {
            string path = ModuleRunTimeState.ModuleBasePath;
            return JsonUtilities.DeserializeFile<ModuleSettings>($"{path}\\ModuleSettings.json");
        }
    }
}