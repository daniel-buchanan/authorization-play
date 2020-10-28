using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class PermissionGrantTag
    {
        public int PermissionGrantId { get; set; }
        public PermissionGrant Grant { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionGrantTag>()
                .HasKey(g => new
                {
                    g.PermissionGrantId,
                    g.TagId
                });
        }
    }
}
