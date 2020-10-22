using System;
using authorization_play.Core.Resources.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Converters
{
    public class ResourceActionConverter : JsonConverter<ResourceAction>
    {
        public override void WriteJson(JsonWriter writer, ResourceAction value, JsonSerializer serializer)
        {
            if (value == null) return;
            writer.WriteValue(value.ToString());
        }

        public override ResourceAction ReadJson(JsonReader reader, Type objectType, ResourceAction existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = reader.Value as string;
            return ResourceAction.FromValue(value);
        }
    }
}