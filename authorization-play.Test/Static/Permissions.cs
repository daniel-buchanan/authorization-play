using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Test.Static
{
    public static class Permissions
    {
        public static PermissionGrant AdminOwner => PermissionGrant.From(Identities.Platform)
            .To(Identities.Admin)
            .WithSchema(Schemas.MilkPickup)
            .WithActions(ResourceActions.Iam.Owner)
            .ForResources(Resources.Herd.Identifier);

        public static PermissionGrant AndreOwner => PermissionGrant.From(Identities.Platform)
            .To(Identities.Andre)
            .WithSchema(Schemas.MilkPickup)
            .WithActions(ResourceActions.Iam.Owner)
            .ForResources(Resources.HerdTwo.Identifier);

        public static PermissionGrant DanielMilkPickup => PermissionGrant.From(Identities.Admin)
            .To(Identities.DanielB)
            .WithSchema(Schemas.MilkPickup)
            .WithActions(ResourceActions.Data.Read)
            .ForResources(Resources.Herd.Identifier);

        public static IPermissionGrantStorage Setup(this IPermissionGrantStorage storage)
        {
            storage.Add(AdminOwner);
            storage.Add(AndreOwner);
            storage.Add(DanielMilkPickup);
            return storage;
        }
    }
}