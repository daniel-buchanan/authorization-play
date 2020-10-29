using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class PrincipalRelation
    {
        public int PrimaryPrincipalId { get; set; }
        public Principal Primary { get; set; }
        public int SecondaryPrincipalId { get; set; }
        public Principal Secondary { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<PrincipalRelation>();
            entityBuilder.HasKey(x => new
            {
                x.PrimaryPrincipalId,
                x.SecondaryPrincipalId
            });
            entityBuilder.HasOne(x => x.Primary)
                .WithMany(x => x.PrimaryRelations);
            entityBuilder.HasOne(x => x.Secondary)
                .WithMany(x => x.SecondaryRelations);
        }
    }
}
