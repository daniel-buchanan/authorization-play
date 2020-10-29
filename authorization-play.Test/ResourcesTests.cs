using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Resources;
using authorization_play.Core.Resources.Models;
using authorization_play.Test.Mocks;
using authorization_play.Test.Static;
using FluentAssertions;
using Xunit;

namespace authorization_play.Test
{
    public class ResourcesTests
    {
        [Theory]
        [MemberData(nameof(Actions))]
        public void AllActionsAreValid(ResourceAction action)
        {
            // Arrange
            var storage = new MockResourceStorage().Setup();
            var resourceValidator = new ResourceValidator(storage);

            // Act
            var result = resourceValidator.ValidateAction(action);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> Actions => ResourceActions.All().Select(a => new object[] { a });

        [Theory]
        [MemberData(nameof(ValidResources))]
        public void AllResourcesAreValid(Resource resource)
        {
            // Arrange
            var storage = new MockResourceStorage().Setup();
            var resourceValidator = new ResourceValidator(storage);

            // Act
            var result = resourceValidator.Validate(resource.Identifier);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> ValidResources => Resources.All().Select(r => new object[] { r });

        [Theory]
        [MemberData(nameof(InvalidResources))]
        public void AllResourcesAreInvalid(CRN resource)
        {
            // Arrange
            var storage = new MockResourceStorage().Setup();
            var resourceValidator = new ResourceValidator(storage);

            // Act
            var result = resourceValidator.Validate(resource);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        public static IEnumerable<object[]> InvalidResources
        {
            get
            {
                yield return new object[] { CRN.FromValue(string.Empty) };
                yield return new object[] { CRN.FromValue("crn:") };
                yield return new object[] { CRN.FromValue("crn:*") };
                yield return new object[] { CRN.FromValue("crn:farm") };
                yield return new object[] { CRN.FromValue("crn:farm:*") };
                yield return new object[] { CRN.FromValue("crn:farm/*") };
                yield return new object[] { CRN.FromValue("crn:farm/1234:herd:*") };
                yield return new object[] { CRN.FromValue("crn:farm/1234:herd/*") };
                yield return new object[] { CRN.FromValue("crn:farm/1234:*") };
            }
        }

        [Theory]
        [MemberData(nameof(ValidResourcesAndActions))]
        public void AllActionsValidForResources(CRN resource, ResourceAction action)
        {
            // Arrange
            var storage = new MockResourceStorage().Setup();
            var resourceValidator = new ResourceValidator(storage);

            // Act
            var result = resourceValidator.Validate(resource, action);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> ValidResourcesAndActions
        {
            get
            {
                yield return new object[] { Resources.Farm.Identifier, ResourceActions.Iam.Owner };
                yield return new object[] { Resources.Herd.Identifier, ResourceActions.Iam.Owner };
                yield return new object[] { Resources.Herd.Identifier, ResourceActions.Identified.Individual };
                yield return new object[] { Resources.HerdAnimals.Identifier, ResourceActions.Iam.Owner };
                yield return new object[] { Resources.HerdAnimals.Identifier, ResourceActions.Identified.Individual };
                yield return new object[] { Resources.HerdAnimals.Identifier, ResourceActions.Identified.Aggregated };
            }
        }

        [Theory]
        [MemberData(nameof(InvalidResourcesAndActions))]
        public void AllActionsInvalidForResources(CRN resource, ResourceAction action)
        {
            // Arrange
            var storage = new MockResourceStorage().Setup();
            var resourceValidator = new ResourceValidator(storage);

            // Act
            var result = resourceValidator.Validate(resource, action);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        public static IEnumerable<object[]> InvalidResourcesAndActions
        {
            get
            {
                yield return new object[] { Resources.Farm.Identifier, ResourceActions.Iam.Delegated };
                yield return new object[] { Resources.Herd.Identifier, ResourceActions.Iam.Delegated };
                yield return new object[] { Resources.Herd.Identifier, ResourceActions.Anonymous.IdentityRemoved};
                yield return new object[] { Resources.HerdAnimals.Identifier, ResourceActions.Iam.Delegated };
                yield return new object[] { Resources.HerdAnimals.Identifier, ResourceActions.Anonymous.IdentityRemoved };
                yield return new object[] { Resources.HerdAnimals.Identifier, ResourceActions.Anonymous.Aggregated };
            }
        }
    }
}
