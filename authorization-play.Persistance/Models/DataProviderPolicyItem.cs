using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class DataProviderPolicyItem
    {
        public int DataProviderPolicyId { get; set; }
        public DataProviderPolicy Policy { get; set; }
        public int PrincipalId { get; set; }
        public Principal Principal { get; set; }
        public bool Allow { get; set; }
        public bool Deny { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataProviderPolicyItem>()
                .HasKey(x => new
                {
                    x.DataProviderPolicyId,
                    x.PrincipalId
                });
        }
    }
}
