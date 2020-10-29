using System.Collections.Generic;

namespace authorization_play.Persistance.Models
{
    public class DataProviderPolicy
    {
        public int DataProviderPolicyId { get; set; }
        public int DataProviderId { get; set; }
        public DataProvider Provider { get; set; }
        public int SchemaId { get; set; }
        public Schema Schema { get; set; }
        public List<DataProviderPolicyItem> PolicyItems { get; set; }
    }
}
