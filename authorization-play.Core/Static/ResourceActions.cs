using System.Collections.Generic;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Core.Static
{
    public static class ResourceActions
    {
        public static IEnumerable<ResourceAction> All()
        {
            yield return Iam.Owner;
            yield return Iam.Delegated;
            yield return Anonymous.Aggregated;
            yield return Anonymous.IdentityRemoved;
            yield return Identified.Aggregated;
            yield return Identified.Individual;
        }

        public class Iam
        {
            public static ResourceAction Owner => ResourceAction.FromValue("iam:owner");
            public static ResourceAction Delegated => ResourceAction.FromValue("iam:delegated");
        }

        public class Anonymous
        {
            public static ResourceAction Aggregated => ResourceAction.FromValue("anon:aggregated");
            public static ResourceAction IdentityRemoved => ResourceAction.FromValue("anon:ident-removed");
        }

        public class Identified
        {
            public static ResourceAction Individual => ResourceAction.FromValue("ident:individual");
            public static ResourceAction Aggregated => ResourceAction.FromValue("ident:aggregated");
        }
    }
}
