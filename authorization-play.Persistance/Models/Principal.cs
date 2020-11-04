using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class Principal
    {
        public int PrincipalId { get; set; }
        public string CanonicalName { get; set; }
        public List<PrincipalRelation> ChildRelations { get; set; }
        public List<PrincipalRelation> ParentRelations { get; set; }
    }
}
