using System;
using authorization_play.Core.Models;
using authorization_play.Core.Principals.Models;
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
                var value = serializer.Deserialize<CPN>(token.CreateReader());
                return Principal.From(value);
            }

            try
            {
                if (token.Value<CRN>() == default(CPN)) return null;
                var value = serializer.Deserialize<CPN>(reader);
                return Principal.From(value);
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
