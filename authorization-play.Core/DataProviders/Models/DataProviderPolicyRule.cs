using authorization_play.Core.Models;

namespace authorization_play.Core.DataProviders.Models
{
    public class DataProviderPolicyRule
    {
        public CRN Principal { get; set; }
        public bool Allow { get; set; }
        public bool Deny { get; set; }

        public static DataProviderPolicyRule ForPrincipal(CRN principal) => new DataProviderPolicyRule()
        {
            Principal = principal
        };

        public DataProviderPolicyRule ExplicitAllow()
        {
            Allow = true;
            Deny = false;
            return this;
        }

        public DataProviderPolicyRule ExplcitDeny()
        {
            Allow = false;
            Deny = true;
            return this;
        }
    }
}
