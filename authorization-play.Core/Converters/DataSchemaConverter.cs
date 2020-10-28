using System;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Converters
{
    public class DataSchemaConverter : JsonConverter<DataSchema>
    {
        public override void WriteJson(JsonWriter writer, DataSchema value, JsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(value.Value)) return;
            writer.WriteValue(value.ToString());
        }

        public override DataSchema ReadJson(JsonReader reader, Type objectType, DataSchema existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrWhiteSpace(value)) return null;
            return DataSchema.FromValue(value);
        }
    }
}