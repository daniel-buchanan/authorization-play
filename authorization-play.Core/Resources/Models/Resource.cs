using System.Collections.Generic;
using authorization_play.Core.Converters;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Resources.Models
{
    [JsonConverter(typeof(ResourceConverter))]
    public class Resource
    {
        public Resource() { }

        public Resource(CRN identifier)
        {
            Identifier = identifier;
            ValidActions = new List<ResourceAction>();
        }

        [JsonProperty("identifier")]
        public CRN Identifier { get; }

        [JsonProperty("action")]
        [JsonConverter(typeof(SingleOrArrayConverter<ResourceAction>))]
        public List<ResourceAction> ValidActions { get; }

        public static bool operator ==(Resource a, Resource b) => a?.Identifier == b?.Identifier;

        public static bool operator !=(Resource a, Resource b) => !(a == b);

        public override int GetHashCode() => Identifier?.GetHashCode() ?? -1;

        public override bool Equals(object obj) => this.Equals(obj as Resource);

        public bool Equals(Resource resource) => this == resource;

        public static Resource FromIdentifier(string identifier) => FromIdentifier(CRN.FromValue(identifier));
        public static Resource FromIdentifier(CRN identifier) => new Resource(identifier);

        public Resource WithActions(params ResourceAction[] actions)
        {
            if (actions == null) return this;
            ValidActions.AddRange(actions);
            return this;
        }
    }
}
