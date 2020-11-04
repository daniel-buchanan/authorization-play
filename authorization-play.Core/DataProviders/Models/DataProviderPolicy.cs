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

        public static DataProviderPolicy ForProvider(CRN provider) => new DataProviderPolicy()
        {
            Provider = provider
        };

        public DataProviderPolicy WithSchema(DataSchema schema)
        {
            Schema = schema;
            return this;
        }

        public DataProviderPolicy WithRule(DataProviderPolicyRule rule)
        {
            if (Rule == null) Rule = new List<DataProviderPolicyRule>();
            Rule.Add(rule);
            return this;
        }
    }
}
