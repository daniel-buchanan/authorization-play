using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class DataSourceResourceConnection
    {
        public int DataSourceConnectionId { get; set; }
        public DataSourceConnection Connection { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }
        public int SchemaId { get; set; }
        public Schema Schema { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DataSourceResourceConnection>();
            entity.HasKey(x => new
            {
                x.DataSourceConnectionId,
                x.ResourceId,
                x.SchemaId
            });
        }
    }
}
