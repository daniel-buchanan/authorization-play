using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace authorization_play.Core.Converters
{
    public class SingleOrArrayConverter<T> : JsonConverter where T: class
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Null) return null;

            if (token.Type == JTokenType.Array)
            {
                if (token.Values<T>() == null) return null;
                return serializer.Deserialize<List<T>>(token.CreateReader());
            }

            if (token.Type == JTokenType.Object)
            {
                var obj = token.ToObject<T>();
                if (obj == null) return null;
                return new List<T>() { obj };
            }

            if (token.Type == JTokenType.String)
            {
                var value = serializer.Deserialize<T>(token.CreateReader());
                return new List<T>() {value};
            }

            try
            {
                if (token.Value<T>() == null) return null;
                return new List<T> { serializer.Deserialize<T>(reader) };
            }
            catch
            {
                try
                {
                    if (token.Values<T>() == null) return null;
                    return token.Values<T>().ToList();
                }
                catch
                {
                    return null;
                }
            }
        }

        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var list = value as List<T>;

            if (list == null)
            {
                writer.WriteValue(new List<T>());
                return;
            }

            if (list.Count == 1)
            {
                serializer.Serialize(writer, list[0]);
                return;
            }

            writer.WriteStartArray();
            foreach(var item in list)
                serializer.Serialize(writer, item);

            writer.WriteEndArray();
        }
    }
}
