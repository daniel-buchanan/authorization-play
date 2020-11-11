using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class Schema
    {
        public int SchemaId { get; set; }
        public string CanonicalName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<DataSourceResourceConnection> Connections { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Schema>();
            entity.HasKey(x => x.SchemaId);

            entity.HasMany(x => x.Connections)
                .WithOne(x => x.Schema);
        }
    }
}
