namespace VstsModuleManagementCore.Utilities
{
    using System.IO;

    using Newtonsoft.Json;

    public static class JsonUtilities
    {
        public static T DeserializeFile<T>(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonReader reader = new JsonTextReader(file);
                return serializer.Deserialize<T>(reader);
            }
        }
    }
}