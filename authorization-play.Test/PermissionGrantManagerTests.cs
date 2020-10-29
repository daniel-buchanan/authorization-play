using System.Collections.Generic;
using authorization_play.Core.DataProviders;
using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;
using authorization_play.Test.Mocks;
using authorization_play.Test.Static;
using FluentAssertions;
using Xunit;

namespace authorization_play.Test
{
    public class PermissionGrantManagerTests
    {
        private readonly IPermissionGrantManager manager;

        public PermissionGrantManagerTests()
        {
            var dataProviderStorage = new MockDataProviderStorage().Setup();
            var applicator = new DataProviderPolicyApplicator(dataProviderStorage);
            var permissionGrantStorage = new MockPermissionGrantStorage().Setup();
            this.manager = new PermissionGrantManager(applicator, permissionGrantStorage);
        }

        [Theory]
        [MemberData(nameof(Rejected))]
        public void GrantRejected(PermissionGrant grant)
        {
            // Act
            var result = this.manager.Add(grant);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(Allowed))]
        public void GrantAllowed(PermissionGrant grant)
        {
            // Act
            var result = this.manager.Add(grant);

            // Assert
            result.Should().BeTrue();
        }

        public static IEnumerable<object[]> Allowed()
        {
            yield return new object[]
            {
                PermissionGrant.For(Identities.Admin)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.Fonterra.Identifier)
                    .WithSchema(Schemas.MilkPickup)
            };

            yield return new object[]
            {
                PermissionGrant.For(Identities.DanielB)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.OpenCountry.Identifier)
                    .WithSchema(Schemas.MilkPickup)
            };

            yield return new object[]
            {
                PermissionGrant.For(Identities.DanielB)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .WithSchema(Schemas.MilkPickup)
            };

            yield return new object[]
            {
                PermissionGrant.For(Identities.Admin)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .WithSchema(Schemas.MilkPickup)
            };

            yield return new object[]
            {
                PermissionGrant.For(Identities.Admin)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.Fonterra.Identifier)
                    .WithSchema(Schemas.NumbersOnProperty)
            };

            yield return new object[]
            {
                PermissionGrant.For(Identities.Admin)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.OpenCountry.Identifier)
                    .WithSchema(Schemas.NumbersOnProperty)
            };

            yield return new object[]
            {
                PermissionGrant.For(Identities.DanielB)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.Fonterra.Identifier)
                    .WithSchema(Schemas.NumbersOnProperty)
            };

            yield return new object[]
            {
                PermissionGrant.For(Identities.DanielB)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.OpenCountry.Identifier)
                    .WithSchema(Schemas.NumbersOnProperty)
            };
        }

        public static IEnumerable<object[]> Rejected()
        {
            yield return new object[]
            {
                PermissionGrant.For(Identities.DanielB)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.Fonterra.Identifier)
                    .WithSchema(Schemas.MilkPickup)
            };

            yield return new object[]
            {
                PermissionGrant.For(Identities.Admin)
                    .ForResources(Resources.Farm.Identifier)
                    .WithActions(ResourceActions.Iam.Owner)
                    .ForSources(DataProviders.OpenCountry.Identifier)
                    .WithSchema(Schemas.MilkPickup)
            };
        }
    }
}
