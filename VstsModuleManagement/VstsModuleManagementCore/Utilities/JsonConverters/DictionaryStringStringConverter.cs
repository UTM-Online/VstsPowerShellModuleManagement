namespace VstsModuleManagementCore.Utilities.JsonConverters
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class DictionaryStringStringConverter : JsonConverter<Dictionary<string,string>>
    {
        public override void WriteJson(JsonWriter writer, Dictionary<string, string> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            foreach (var item in value)
            {
                serializer.Serialize(writer, item);
            }

            writer.WriteEndArray();
        }

        public override Dictionary<string, string> ReadJson(
            JsonReader reader,
            Type objectType,
            Dictionary<string, string> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray array = JArray.Load(reader);

                List<KeyValuePair<string,string>> target = new List<KeyValuePair<string, string>>();

                serializer.Populate(array.CreateReader(), target);

                if (existingValue == null)
                {
                    existingValue = new Dictionary<string, string>();
                }

                foreach (var item in target)
                {
                    if (!existingValue.ContainsKey(item.Key))
                    {
                        existingValue.Add(item.Key, item.Value);
                    }
                }

                return existingValue;
            }

            return new Dictionary<string, string>();
        }
    }
}