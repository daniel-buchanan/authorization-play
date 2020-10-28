using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class DelegationGrantTag
    {
        public int DelegationGrantId { get; set; }
        public DelegationGrant Grant { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DelegationGrantTag>()
                .HasKey(g => new
                {
                    g.DelegationGrantId,
                    g.TagId
                });
        }
    }
}
