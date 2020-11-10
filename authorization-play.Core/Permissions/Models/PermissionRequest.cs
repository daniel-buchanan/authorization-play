using System;
using System.Collections.Generic;
using authorization_play.Core.Converters;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Permissions.Models
{
    public class PermissionRequest
    {
        [JsonProperty("resource")]
        [JsonConverter(typeof(SingleOrArrayConverter<PermissionResourceRequest>))]
        public List<PermissionResourceRequest> Resources { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("rat")]
        [JsonConverter(typeof(DateTimeToEpochConverter))]
        public DateTimeOffset RequestedAt { get; set; }

        [JsonProperty("req")]
        public CPN RequestingPrincipal { get; set; }
    }

    public class PermissionResourceRequest
    {
        [JsonProperty("resource")]
        public CRN Resource { get; set; }
        
        [JsonProperty("action")]
        [JsonConverter(typeof(SingleOrArrayConverter<ResourceAction>))]
        public List<ResourceAction> Actions { get; set; }

        [JsonProperty("schema")]
        [JsonConverter(typeof(SingleOrArrayConverter<CSN>))]
        public List<CSN> Schemas { get; set; }
    }
}
