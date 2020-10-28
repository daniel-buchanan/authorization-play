using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class PermissionGrantResourceAction
    {
        public int PermissionGrantId { get; set; }
        public PermissionGrant Grant { get; set; }
        public int ActionId { get; set; }
        public Action Action { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionGrantResourceAction>()
                .HasKey(g => new
                {
                    g.PermissionGrantId,
                    g.ActionId
                });
        }
    }
}
