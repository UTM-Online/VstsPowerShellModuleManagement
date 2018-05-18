namespace VstsModuleManagementCore.Utilities
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    using VstsModuleManagementCore.Utilities.JsonConverters;

    public static class JsonUtilities
    {
        public static T DeserializeFile<T>(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new DictionaryStringStringCreationConverter<string>(StringComparer.OrdinalIgnoreCase));
                JsonReader reader = new JsonTextReader(file);
                return serializer.Deserialize<T>(reader);
            }
        }
    }
}