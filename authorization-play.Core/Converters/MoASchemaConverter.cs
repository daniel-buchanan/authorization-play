using System;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Converters
{
    public class MoASchemaConverter : JsonConverter<MoASchema>
    {
        public override void WriteJson(JsonWriter writer, MoASchema value, JsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(value.Value)) return;
            writer.WriteValue(value.ToString());
        }

        public override MoASchema ReadJson(JsonReader reader, Type objectType, MoASchema existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrWhiteSpace(value)) return null;
            return MoASchema.FromValue(value);
        }
    }
}