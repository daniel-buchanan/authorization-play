using authorization_play.Core.DataProviders.Models;

namespace authorization_play.Test.Static
{
    public static class DataProviderPolicies
    {
        public static DataProviderPolicy Fonterra => DataProviderPolicy
            .ForProvider(DataProviders.Fonterra.Identifier)
            .WithSchema(Schemas.MilkPickup)
            .WithRule(DataProviderPolicyRule
                .ForPrincipal(Identities.DanielB)
                .ExplcitDeny())
            .WithRule(DataProviderPolicyRule
                .ForPrincipal(Identities.Admin)
                .ExplicitAllow());

        public static DataProviderPolicy OpenCountry => DataProviderPolicy
            .ForProvider(DataProviders.OpenCountry.Identifier)
            .WithSchema(Schemas.MilkPickup)
            .WithRule(DataProviderPolicyRule
                .ForPrincipal(Identities.Admin)
                .ExplcitDeny())
            .WithRule(DataProviderPolicyRule
                .ForPrincipal(Identities.DanielB)
                .ExplicitAllow());
    }
}
