using authorization_play.Persistance.Models;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Persistance
{
    public class AuthorizationPlayContext : DbContext
    {
        public DbSet<DelegationGrant> DelegationGrants { get; set; }
        public DbSet<DelegationGrantResource> DelegationGrantResources { get; set; }
        public DbSet<DelegationGrantResourceAction> DelegationGrantResourceActions { get; set; }
        public DbSet<DelegationGrantTag> DelegationGrantTags { get; set; }
        public DbSet<PermissionGrant> PermissionGrants { get; set; }
        public DbSet<PermissionGrantResource> PermissionGrantResources { get; set; }
        public DbSet<PermissionGrantResourceAction> PermissionGrantResourceActions { get; set; }
        public DbSet<PermissionGrantTag> PermissionGrantTags { get; set; }
        public DbSet<Principal> Principals { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceKind> ResourceKinds { get; set; }
        public DbSet<ResourceAction> ResourceActions { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Schema> Schemas { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql("Data Source=blogging.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ResourceAction.OnModelCreating(modelBuilder);

            DelegationGrantResource.OnModelCreating(modelBuilder);
            DelegationGrantResourceAction.OnModelCreating(modelBuilder);
            DelegationGrantTag.OnModelCreating(modelBuilder);

            PermissionGrantResource.OnModelCreating(modelBuilder);
            PermissionGrantResourceAction.OnModelCreating(modelBuilder);
            PermissionGrantTag.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
