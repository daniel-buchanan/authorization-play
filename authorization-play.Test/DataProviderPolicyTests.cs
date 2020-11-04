using System.Collections.Generic;
using authorization_play.Core.DataProviders;
using authorization_play.Core.Permissions.Models;
using authorization_play.Test.Mocks;
using authorization_play.Test.Static;
using FluentAssertions;
using Xunit;

namespace authorization_play.Test
{
    public class DataProviderPolicyTests
    {
        private readonly IDataProviderPolicyApplicator applicator;

        public DataProviderPolicyTests()
        {
            var storage = new MockDataProviderStorage().Setup();
            this.applicator = new DataProviderPolicyApplicator(storage);
        }

        [Theory]
        [MemberData(nameof(DeniedGrants))]
        public void PolicyDeniesGrant(PermissionGrant grant)
        {
            // Act
            var valid = this.applicator.IsGrantValid(grant);

            // Assert
            valid.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(AllowedGrants))]
        public void PolicyAllowsGrant(PermissionGrant grant)
        {
            // Act
            var valid = this.applicator.IsGrantValid(grant);

            // Assert
            valid.Should().BeTrue();
        }

        public static IEnumerable<object[]> AllowedGrants()
        {
            yield return new object[]
            {
                PermissionGrant.From(Identities.Platform)
                    .To(Identities.Admin)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.Fonterra.Identifier)
                    .WithSchema(Schemas.MilkPickup)
            };

            yield return new object[]
            {
                PermissionGrant.From(Identities.Platform)
                    .To(Identities.DanielB)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.OpenCountry.Identifier)
                    .WithSchema(Schemas.MilkPickup)
            };

            yield return new object[]
            {
                PermissionGrant.From(Identities.Platform)
                    .To(Identities.DanielB)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .WithSchema(Schemas.MilkPickup)
            };

            yield return new object[]
            {
                PermissionGrant.From(Identities.Platform)
                    .To(Identities.Admin)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .WithSchema(Schemas.MilkPickup)
            };
        }

        public static IEnumerable<object[]> DeniedGrants()
        {
            yield return new object[]
            {
                PermissionGrant.From(Identities.Platform)
                    .To(Identities.DanielB)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.Fonterra.Identifier)
                    .WithSchema(Schemas.MilkPickup)
            };

            yield return new object[]
            {
                PermissionGrant.From(Identities.Platform)
                    .To(Identities.Admin)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.OpenCountry.Identifier)
                    .WithSchema(Schemas.MilkPickup)
            };
        }
    }
}
