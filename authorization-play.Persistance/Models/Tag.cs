using System.Collections.Generic;

namespace authorization_play.Persistance.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string CanonicalName { get; set; }
        public string DisplayName { get; set; }
        public List<PermissionGrant> PermissionGrants { get; set; }
        public List<DelegationGrant> DelegationGrants { get; set; }
    }
}
