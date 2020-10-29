using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class Principal
    {
        public int PrincipalId { get; set; }
        public string CanonicalName { get; set; }
        public List<PrincipalRelation> PrimaryRelations { get; set; }
        public List<PrincipalRelation> SecondaryRelations { get; set; }
    }
}
