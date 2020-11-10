using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Converters;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Permissions.Models
{
    public class PermissionTicketResource
    {
        public PermissionTicketResource()
        {
            Actions = new List<ResourceAction>();
        }

        [JsonProperty("resource")]
        public CRN Identifier { get; set; }

        [JsonProperty("schema")]
        public CSN Schema { get; set; }

        [JsonProperty("action")]
        [JsonConverter(typeof(SingleOrArrayConverter<ResourceAction>))]
        public List<ResourceAction> Actions { get; set; }

        [JsonIgnore]
        public bool IsValid => Identifier?.IsValid == true &&
                               Schema?.IsValid == true &&
                               Actions.Any();

        public static PermissionTicketResource ForResource(CRN resource) => new PermissionTicketResource()
        {
            Identifier = resource
        };

        public PermissionTicketResource ForSchema(CSN schema)
        {
            Schema = schema;
            return this;
        }

        public PermissionTicketResource WithActions(params ResourceAction[] actions)
        {
            if (actions == null || actions.Length == 0) return this;
            if (!Actions.Any()) Actions = actions.ToList();
            else Actions.AddRange(actions);
            return this;
        }
    }
}