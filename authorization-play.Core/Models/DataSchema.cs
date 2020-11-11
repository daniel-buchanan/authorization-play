using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    public class DataSchema
    {
        [JsonProperty("identifier")]
        public CSN Identifier { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
