using System.Collections.Generic;
using authorization_play.Core.Resources;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Test.Static
{
    public static class Resources
    {
        public static IEnumerable<Resource> All()
        {
            yield return Farm;
            yield return FarmTwo;
            yield return Herd;
            yield return HerdTwo;
            yield return HerdAnimals;
        }

        public static Resource Farm => Resource.FromIdentifier("crn:farm/1234").WithActions(ResourceActions.Iam.Owner);
        public static Resource FarmTwo => Resource.FromIdentifier("crn:farm/1231").WithActions(ResourceActions.Iam.Owner);
        public static Resource Herd => Resource.FromIdentifier("crn:farm/1234:herd/88756").WithActions(ResourceActions.Iam.Owner, ResourceActions.Identified.Individual);
        public static Resource HerdTwo => Resource.FromIdentifier("crn:farm/1234:herd/88722").WithActions(ResourceActions.Iam.Owner, ResourceActions.Identified.Individual);
        public static Resource HerdAnimals => Resource.FromIdentifier("crn:farm/1234:herd/88756:animals").WithActions(ResourceActions.Iam.Owner, ResourceActions.Identified.Individual, ResourceActions.Identified.Aggregated);

        public static IResourceStorage Setup(this IResourceStorage storage)
        {
            foreach(var r in All())
                storage.Add(r);

            foreach(var a in ResourceActions.All())
                storage.AddAction(a);

            return storage;
        }
    }
}