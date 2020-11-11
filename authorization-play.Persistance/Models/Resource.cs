using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string CanonicalName { get; set; }
        public List<ResourceAction> Actions { get; set; }
        public List<DataSourceResourceConnection> Connections { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Resource>();
            entity.HasKey(x => x.ResourceId);

            entity.HasMany(x => x.Actions)
                .WithOne(x => x.Resource);

            entity.HasMany(x => x.Connections)
                .WithOne(x => x.Resource);
        }
    }
}
