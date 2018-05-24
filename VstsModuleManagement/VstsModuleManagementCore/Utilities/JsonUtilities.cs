namespace VstsModuleManagementCore.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    using VstsModuleManagementCore.Models;
    using VstsModuleManagementCore.Resources;
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

        internal static List<BlackListedCommandCollection.BlackListedCommand> GetBlackListedCommands()
        {
            using (var streamReader = new StreamReader(FileUtilities.GetUnmanagedMemoryStream("BlackListedCommands.json")))
            {
                var serializer = new JsonSerializer();
                var jsonReader = new JsonTextReader(streamReader);
                var deserializedObject = serializer.Deserialize<BlackListedCommandCollection>(jsonReader);
                return deserializedObject.BlackListedCommands;
            }
        }
    }
}