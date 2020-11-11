using System;
using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace authorization_play.Core.Converters
{
    public class ResourceConverter : JsonConverter<Resource>
    {
        public override Resource ReadJson(JsonReader reader, Type objectType, Resource existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (token.Type == JTokenType.Object)
            {
                CRN identifier = null;
                ResourceAction[] actions = null;
                foreach (var t in token.Children())
                {
                    if (t.Path == "identifier")
                    {
                        identifier = CRN.FromValue(t.First.Value<string>());
                        continue;
                    }

                    if (t.Path == "action")
                    {
                        var values = t.Values();
                        actions = values.Select(v => ResourceAction.FromValue(v.Value<string>())).ToArray();
                        continue;
                    }
                }

                if (actions != null) return Resource.FromIdentifier(identifier).WithActions(actions);
                return Resource.FromIdentifier(identifier);
            }

            if (token.Type == JTokenType.String)
            {
                var value = serializer.Deserialize<CRN>(token.CreateReader());
                return Resource.FromIdentifier(value);
            }

            try
            {
                if (token.Value<CRN>() == default) return null;
                var value = serializer.Deserialize<CRN>(reader);
                return Resource.FromIdentifier(value);
            }
            catch
            {
                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, Resource value, JsonSerializer serializer)
        {
            if (value.ValidActions == null || value.ValidActions.Count == 0)
            {
                serializer.Serialize(writer, value.Identifier);
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName("identifier");
            writer.WriteValue(value.Identifier);
            writer.WritePropertyName("action");
            serializer.Serialize(writer, value.ValidActions);
            writer.WriteEndObject();
        }

        public override bool CanWrite => true;
    }
}
