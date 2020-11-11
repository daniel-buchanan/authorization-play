using System.Collections.Generic;
using authorization_play.Core.Converters;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.DataProviders.Models
{
    public class DataSource
    {
        [JsonProperty("provider")]
        public CRN Provider { get; set; }

        [JsonProperty("identifier")]
        public CRN Identifier { get; set; }

        [JsonProperty("schema")]
        [JsonConverter(typeof(SingleOrArrayConverter<DataSchema>))]
        public List<DataSchema> Schemas { get; set; }
    }
}
