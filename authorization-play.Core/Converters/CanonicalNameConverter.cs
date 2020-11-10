using System;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Converters
{
    public class CanonicalNameConverter<T> : JsonConverter<T> where T: CanonicalName, new()
    {
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(value.Value)) return;
            writer.WriteValue(value.ToString());
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrWhiteSpace(value)) return null;
            return CanonicalName.FromValue<T>(value);
        }
    }
}