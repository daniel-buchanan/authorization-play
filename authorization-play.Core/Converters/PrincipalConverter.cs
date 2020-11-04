using System;
using authorization_play.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace authorization_play.Core.Converters
{
    public class PrincipalConverter : JsonConverter<Principal>
    {
        public override Principal ReadJson(JsonReader reader, Type objectType, Principal existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (token.Type == JTokenType.Object)
            {
                return serializer.Deserialize<Principal>(reader);
            }

            if (token.Type == JTokenType.String)
            {
                var value = serializer.Deserialize<CRN>(token.CreateReader());
                return new Principal() { Identifier = value };
            }

            try
            {
                if (token.Value<CRN>() == default(CRN)) return null;
                return new Principal()
                {
                    Identifier = serializer.Deserialize<CRN>(reader)
                };
            }
            catch
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, Principal value, JsonSerializer serializer)
        {
            if (value.Children == null || value.Children.Count == 0)
            {
                serializer.Serialize(writer, value.Identifier);
                return;
            }

            serializer.Serialize(writer, value);
        }

        public override bool CanWrite => true;
    }
}
