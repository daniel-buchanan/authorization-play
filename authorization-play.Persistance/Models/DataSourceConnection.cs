using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class DataSourceConnection
    {
        public int DataSourceConnectionId { get; set; }
        public int DataSourceId { get; set; }
        public DataSource DataSource { get; set; }
        public int DataProviderId { get; set; }
        public DataProvider Provider { get; set; }
        public List<DataSourceResourceConnection> Resources { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<DataSourceConnection>();
            entity.HasKey(x => x.DataSourceConnectionId);

            entity.HasMany(x => x.Resources)
                .WithOne(x => x.Connection);
        }
    }
}
