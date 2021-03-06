﻿using System.Collections.Generic;
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
    public class ResourceFinderTests
    {
        [Theory]
        [MemberData(nameof(AllKnownResources))]
        public void FindSpecificResource(CRN rn)
        {
            // Arrange
            var storage = new MockResourceStorage().Setup();
            var resourceFinder = new ResourceFinder(storage);

            // Act
            var result = resourceFinder.Find(rn);

            // Assert
            result.Should().ContainSingle(r => r.Identifier == rn);
        }

        public static IEnumerable<object[]> AllKnownResources => Resources.All().Select(r => new object[] { r.Identifier });

        [Theory]
        [MemberData(nameof(UnknownResources))]
        public void ResourceNotFound(CRN rn)
        {
            // Arrange
            var storage = new MockResourceStorage().Setup();
            var resourceFinder = new ResourceFinder(storage);

            // Act
            var result = resourceFinder.Find(rn);

            // Assert
            result.Should().BeEmpty();
        }

        public static IEnumerable<object[]> UnknownResources()
        {
            yield return new object[] { CRN.FromValue("crn:farm/1") };
            yield return new object[] { CRN.FromValue("crn:farm/*:herd/8876:*") };
        }

        [Theory]
        [MemberData(nameof(WildcardResources))]
        public void FindResourcesByWildcard(CRN resource, IEnumerable<Resource> expectedResources)
        {
            // Arrange
            var storage = new MockResourceStorage().Setup();
            var resourceFinder = new ResourceFinder(storage);

            // Act
            var result = resourceFinder.Find(resource);

            // Assert
            result.Should().BeEquivalentTo(expectedResources);
        }

        public static IEnumerable<object[]> WildcardResources
        {
            get
            {
                yield return new object[] { CRN.FromValue("crn:farm/*"), new[] { Resources.Farm, Resources.FarmTwo } };
                yield return new object[] { CRN.FromValue("crn:farm/*:*"), new[] { Resources.Farm, Resources.FarmTwo, Resources.Herd, Resources.HerdTwo, Resources.HerdAnimals } };
                yield return new object[] { CRN.FromValue("crn:farm/*:herd/*"), new[] { Resources.Herd, Resources.HerdTwo } };
                yield return new object[] { CRN.FromValue("crn:farm/*:herd/*:*"), new[] { Resources.Herd, Resources.HerdTwo, Resources.HerdAnimals } };
                yield return new object[] { CRN.FromValue("crn:farm/1234:herd/*"), new[] { Resources.Herd, Resources.HerdTwo } };
                yield return new object[] { CRN.FromValue("crn:farm/1234:herd/*:*"), new[] { Resources.Herd, Resources.HerdTwo, Resources.HerdAnimals } };
            }
        }
    }
}
