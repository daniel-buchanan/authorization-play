using System.Collections.Generic;
using authorization_play.Core.DataProviders;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Resources;
using authorization_play.Test.Mocks;
using authorization_play.Test.Static;
using FluentAssertions;
using Xunit;

namespace authorization_play.Test
{
    public class PermissionValidatorTests
    {
        IPermissionValidator GetValidator()
        {
            var resourceStorage = new MockResourceStorage().Setup();
            var permissionStorage = new MockPermissionGrantStorage().Setup();
            var resourceFinder = new ResourceFinder(resourceStorage);
            var resourceValidator = new ResourceValidator(resourceStorage);
            var permissionFinder = new PermissionGrantFinder(permissionStorage);
            var dataProviderStorage = new MockDataProviderStorage().Setup();
            var principalStorage = new MockPrincipalStorage().Setup();
            var policyApplicator = new DataProviderPolicyApplicator(dataProviderStorage, principalStorage);
            return new PermissionValidator(resourceValidator, resourceFinder, permissionFinder, policyApplicator);
        }

        [Theory]
        [MemberData(nameof(ValidPermissions))]
        public void PermissionValid(PermissionValidationRequest request)
        {
            // Arrange
            var validator = GetValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().Be(PermissionsValid.True);
        }

        public static IEnumerable<object[]> ValidPermissions()
        {
            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Data.Read,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756")
                }
            };

            //yield return new object[]
            //{
            //    new PermissionTicketRequest()
            //    {
            //        Action = ResourceActions.Data.Read,
            //        Principal = Identities.DanielB,
            //        Schema = Schemas.MilkPickup,
            //        Resource = CRN.FromValue("crn:farm/*:herd/88756:*")
            //    }
            //};

            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Data.Read,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/1234:herd/88756")
                }
            };

            //yield return new object[]
            //{
            //    new PermissionTicketRequest()
            //    {
            //        Action = ResourceActions.Data.Read,
            //        Principal = Identities.DanielB,
            //        Schema = Schemas.MilkPickup,
            //        Resource = CRN.FromValue("crn:farm/1234:herd/88756:*")
            //    }
            //};
        }

        [Theory]
        [MemberData(nameof(InvalidPermissions))]
        public void PermissionInvalid(PermissionValidationRequest request)
        {
            // Arrange
            var validator = GetValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().Be(PermissionsValid.False);
        }

        public static IEnumerable<object[]> InvalidPermissions()
        {
            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Identified.Individual,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756")
                }
            };

            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Iam.Owner,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/8876:*")
                }
            };

            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Iam.Delegated,
                    Principal = Identities.Admin,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756")
                }
            };

            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Iam.Delegated,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756")
                }
            };

            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Iam.Owner,
                    Principal = Identities.DanielB,
                    Schema = Schemas.NumbersOnProperty,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756")
                }
            };
        }

        [Theory]
        [MemberData(nameof(PartiallyValidPermissions))]
        public void PermissionPartiallyValid(PermissionValidationRequest request)
        {
            // Arrange
            var validator = GetValidator();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().Be(PermissionsValid.Partial);
        }

        public static IEnumerable<object[]> PartiallyValidPermissions()
        {
            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Data.Read,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/*:*")
                }
            };

            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Data.Read,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/*")
                }
            };

            yield return new object[]
            {
                new PermissionTicketRequest()
                {
                    Action = ResourceActions.Data.Read,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/1234:herd/*")
                }
            };
        }
    }
}
