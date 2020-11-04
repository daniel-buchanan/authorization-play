using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class PrincipalRelation
    {
        public int ParentPrincipalId { get; set; }
        public Principal Parent { get; set; }
        public int ChildPrincipalId { get; set; }
        public Principal Child { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<PrincipalRelation>();
            entityBuilder.HasKey(x => new
            {
                x.ParentPrincipalId,
                x.ChildPrincipalId
            });
            entityBuilder.HasOne(x => x.Parent)
                .WithMany(x => x.ChildRelations);
            entityBuilder.HasOne(x => x.Child)
                .WithMany(x => x.ParentRelations);
        }
    }
}
