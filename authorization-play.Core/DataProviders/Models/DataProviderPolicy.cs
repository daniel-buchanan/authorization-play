using System.Collections.Generic;
using authorization_play.Core.Converters;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.DataProviders.Models
{
    public class DataProviderPolicy
    {
        public CRN Provider { get; set; }
        public DataSchema Schema { get; set; }
        [JsonConverter(typeof(SingleOrArrayConverter<DataProviderPolicyRule>))]
        public List<DataProviderPolicyRule> Rule { get; set; }
    }
}
