using authorization_play.Core.Permissions;
using authorization_play.Test.Mocks;
using authorization_play.Test.Static;
using FluentAssertions;
using Xunit;

namespace authorization_play.Test
{
    public class PermissionGrantFinderTests
    {
        [Fact]
        public void PermissionNotFound()
        {
            // Arrange
            var storage = new MockPermissionGrantStorage().Setup();
            var permissionFinder = new PermissionGrantFinder(storage);

            // Act
            var result = permissionFinder.Find(Identities.Admin, Schemas.NumbersOnProperty);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void PermissionFound()
        {
            // Arrange
            var storage = new MockPermissionGrantStorage().Setup();
            var permissionFinder = new PermissionGrantFinder(storage);

            // Act
            var result = permissionFinder.Find(Identities.DanielB, Schemas.MilkPickup);

            // Assert
            result.Should().Contain(Permissions.DanielMilkPickup);
        }

        [Fact]
        public void PermissionWithoutSchemaNotFound()
        {
            // Arrange
            var storage = new MockPermissionGrantStorage().Setup();
            var permissionFinder = new PermissionGrantFinder(storage);

            // Act
            var result = permissionFinder.Find(Identities.Platform);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void PermissionWithoutSchemaFound()
        {
            // Arrange
            var storage = new MockPermissionGrantStorage().Setup();
            var permissionFinder = new PermissionGrantFinder(storage);

            // Act
            var result = permissionFinder.Find(Identities.DanielB);

            // Assert
            result.Should().Contain(Permissions.DanielMilkPickup);
        }
    }
}
