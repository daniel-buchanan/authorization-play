using System.Collections.Generic;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Test.Static
{
    public static class Permissions
    {
        public static IEnumerable<PermissionGrant> All()
        {
            yield return DanielMilkPickup;
        }

        public static PermissionGrant DanielMilkPickup => PermissionGrant.For(Identities.DanielB)
            .WithSchema(Schemas.MilkPickup)
            .WithActions(ResourceActions.Iam.Owner)
            .ForResources(CRN.FromValue("crn:farm/*:herd/88756:*"));

        public static IPermissionGrantStorage Setup(this IPermissionGrantStorage storage)
        {
            storage.Add(DanielMilkPickup);
            return storage;
        }
    }
}