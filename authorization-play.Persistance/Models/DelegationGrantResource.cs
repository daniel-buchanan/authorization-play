using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class DelegationGrantResource
    {
        public int DelegationGrantId { get; set; }
        public DelegationGrant Grant { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DelegationGrantResource>()
                .HasKey(g => new
                {
                    g.DelegationGrantId,
                    g.ResourceId
                });
        }
    }
}
