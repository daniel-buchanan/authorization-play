using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class ResourceAction
    {
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }
        public int ActionId { get; set; }
        public Action Action { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResourceAction>()
                .HasKey(r => new
                {
                    r.ActionId,
                    r.ResourceId
                });
        }
    }
}
