using System.Collections.Generic;
using authorization_play.Core.Converters;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.DataProviders.Models
{
    public class DataSourceConnection
    {
        [JsonProperty("source")]
        public CRN Source { get; set; }

        [JsonProperty("provider")]
        public CRN Provider { get; set; }

        [JsonProperty("resource")]
        [JsonConverter(typeof(SingleOrArrayConverter<DataSourceResourceConnection>))]
        public List<DataSourceResourceConnection> Schemas { get; set; }
    }

    public class DataSourceResourceConnection
    {
        [JsonProperty("resource")]
        public CRN Resource { get; set; }

        [JsonProperty("schema")]
        public CSN Schema { get; set; }
    }
}
