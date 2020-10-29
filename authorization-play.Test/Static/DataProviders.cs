using authorization_play.Core.DataProviders;
using authorization_play.Core.DataProviders.Models;
using authorization_play.Core.Models;

namespace authorization_play.Test.Static
{
    public static class DataProviders
    {
        public static DataProvider Fonterra => DataProvider.FromIdentifier(CRN.FromValue("crn:provider/1"))
            .ForPrincipal(Identities.Organisations.Fonterra)
            .WithName("Fonterra");

        public static DataProvider OpenCountry => DataProvider.FromIdentifier(CRN.FromValue("crn:provider/2"))
            .ForPrincipal(Identities.Organisations.OpenCountry)
            .WithName("Open Country");

        public static IDataProviderStorage Setup(this IDataProviderStorage storage)
        {
            storage.Add(Fonterra);
            storage.Add(OpenCountry);

            storage.AddPolicy(DataProviderPolicies.Fonterra);
            storage.AddPolicy(DataProviderPolicies.OpenCountry);
            return storage;
        }
    }
}
