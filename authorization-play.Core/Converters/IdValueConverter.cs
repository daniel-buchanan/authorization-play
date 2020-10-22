using System;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Converters
{
    public class IdValueConverter : JsonConverter<IdValue>
    {
        public override void WriteJson(JsonWriter writer, IdValue value, JsonSerializer serializer)
        {
            if(value.Any) writer.WriteValue("*");
            else writer.WriteValue(value.Value);
        }

        public override IdValue ReadJson(JsonReader reader, Type objectType, IdValue existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value as string;
            return IdValue.FromValue(value);
        }
    }
}