using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Converters;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Permissions.Models
{
    public class PermissionGrant
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }

        [JsonProperty("resource")]
        [JsonConverter(typeof(SingleOrArrayConverter<CRN>))]
        public List<CRN> Resource { get; set; }

        [JsonProperty("tag")]
        [JsonConverter(typeof(SingleOrArrayConverter<CRN>))]
        public List<CRN> Tag { get; set; }
        
        [JsonProperty("schema")]
        public CSN Schema { get; set; }
        
        [JsonProperty("principal")]
        public CPN Principal { get; set; }

        [JsonProperty("grantor")]
        public CPN Grantor { get; set; }
        
        [JsonProperty("action")]
        [JsonConverter(typeof(SingleOrArrayConverter<ResourceAction>))]
        public List<ResourceAction> Actions { get; set; }

        public static PermissionGrant From(CPN grantor) => new PermissionGrant() { Grantor = grantor };

        public PermissionGrant To(CPN principal)
        {
            Principal = principal;
            return this;
        }

        public PermissionGrant WithSchema(CSN schema)
        {
            Schema = schema;
            return this;
        }

        public PermissionGrant ForResources(params CRN[] resources)
        {
            Resource = resources?.ToList() ?? new List<CRN>();
            return this;
        }

        public PermissionGrant WithActions(params ResourceAction[] actions)
        {
            Actions = actions?.ToList() ?? new List<ResourceAction>();
            return this;
        }

        public PermissionGrant ForSources(params CRN[] sources)
        {
            Tag = sources?.ToList() ?? new List<CRN>();
            return this;
        }

        public static bool operator ==(PermissionGrant a, PermissionGrant b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;

            return a.Schema == b.Schema &&
                   a.Principal == b.Principal &&
                   ListsEqual(a.Resource, b.Resource) &&
                   ListsEqual(a.Actions, b.Actions) &&
                   ListsEqual(a.Tag, b.Tag);
        }

        public static bool operator !=(PermissionGrant a, PermissionGrant b) => !(a == b);

        private static bool ListsEqual<T>(List<T> a, List<T> b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;

            if (a.Count != b.Count) return false;

            var allSame = true;
            for (var i = 0; i < a.Count; i++)
            {
                if (a[i] == null && b[i] == null) continue;
                if (a[i] == null || b[i] == null) return false;
                allSame &= a[i].Equals(b[i]);
            }

            return allSame;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj) => this.Equals(obj as PermissionGrant);

        public bool Equals(PermissionGrant resource) => this == resource;

        [JsonIgnore]
        public bool IsValid => Resource?.TrueForAll(r => r.IsValid) == true &&
                               Schema?.IsValid == true &&
                               Principal?.IsValid == true &&
                               Grantor?.IsValid == true &&
                               Actions?.Count > 0;
    }
}
