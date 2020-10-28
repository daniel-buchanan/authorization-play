using System.Collections.Generic;

namespace authorization_play.Persistance.Models
{
    public class PermissionGrant
    {
        public int PermissionGrantId { get; set; }
        public int PrincipalId { get; set; }
        public Principal Principal { get; set; }
        public int SchemaId { get; set; }
        public Schema Schema { get; set; }
        public List<PermissionGrantResource> Resources { get; set; }
        public List<PermissionGrantResourceAction> Actions { get; set; }
        public List<PermissionGrantTag> Tags { get; set; }
    }
}
