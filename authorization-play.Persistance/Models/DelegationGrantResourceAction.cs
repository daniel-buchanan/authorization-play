using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class DelegationGrantResourceAction
    {
        public int DelegationGrantId { get; set; }
        public DelegationGrant Grant { get; set; }
        public int ActionId { get; set; }
        public Action Action { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DelegationGrantResourceAction>()
                .HasKey(g => new
                {
                    g.DelegationGrantId,
                    g.ActionId
                });
        }
    }
}
