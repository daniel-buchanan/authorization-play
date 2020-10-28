using System;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Converters
{
    public class CRNConverter : JsonConverter<CRN>
    {
        public override void WriteJson(JsonWriter writer, CRN value, JsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(value.Value)) return;
            writer.WriteValue(value.ToString());
        }

        public override CRN ReadJson(JsonReader reader, Type objectType, CRN existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrWhiteSpace(value)) return null;
            return CRN.FromValue(value);
        }
    }
}