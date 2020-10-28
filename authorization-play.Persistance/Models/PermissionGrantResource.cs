using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class PermissionGrantResource
    {
        public int PermissionGrantId { get; set; }
        public PermissionGrant Grant { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionGrantResource>()
                .HasKey(g => new
                {
                    g.PermissionGrantId,
                    g.ResourceId
                });
        }
    }
}
