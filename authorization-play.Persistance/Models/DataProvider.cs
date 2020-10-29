using System.Collections.Generic;

namespace authorization_play.Persistance.Models
{
    public class DataProvider
    {
        public int DataProviderId { get; set; }
        public int PrincipalId { get; set; }
        public Principal Principal { get; set; }
        public string Name { get; set; }
        public string CanonicalName { get; set; }
        public List<DataSource> DataSources { get; set; }
        public List<DataProviderPolicy> Policies { get; set; }
    }
}
