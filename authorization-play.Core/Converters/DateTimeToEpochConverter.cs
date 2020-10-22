using System;
using Newtonsoft.Json;

namespace authorization_play.Core.Converters
{
    public class DateTimeToEpochConverter : JsonConverter<DateTimeOffset>
    {
        public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToUnixTimeSeconds());
        }

        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value as long?;
            if(value != null) return DateTimeOffset.FromUnixTimeSeconds(value.GetValueOrDefault());
            return DateTimeOffset.MinValue;
        }
    }
}
