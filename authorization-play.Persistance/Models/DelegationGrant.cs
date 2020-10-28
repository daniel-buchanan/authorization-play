using System.Collections.Generic;

namespace authorization_play.Persistance.Models
{
    public class DelegationGrant
    {
        public int DelegationGrantId { get; set; }
        public int PrincipalId { get; set; }
        public Principal Principal { get; set; }
        public int SchemaId { get; set; }
        public Schema Schema { get; set; }
        public List<DelegationGrantResource> Resources { get; set; }
        public List<DelegationGrantResourceAction> ResourceActions { get; set; }
        public List<DelegationGrantTag> Tags { get; set; }
    }
}
