using System;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Converters
{
    public class MoARNConverter : JsonConverter<MoARN>
    {
        public override void WriteJson(JsonWriter writer, MoARN value, JsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(value.Value)) return;
            writer.WriteValue(value.ToString());
        }

        public override MoARN ReadJson(JsonReader reader, Type objectType, MoARN existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrWhiteSpace(value)) return null;
            return MoARN.FromValue(value);
        }
    }
}