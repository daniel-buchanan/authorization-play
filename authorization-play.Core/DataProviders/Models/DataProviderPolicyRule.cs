using authorization_play.Core.Models;

namespace authorization_play.Core.DataProviders.Models
{
    public class DataProviderPolicyRule
    {
        public CRN Principal { get; set; }
        public bool Allow { get; set; }
        public bool Deny { get; set; }
    }
}
