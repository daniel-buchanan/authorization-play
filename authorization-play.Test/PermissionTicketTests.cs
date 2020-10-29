using System;
using authorization_play.Core.Permissions.Models;
using authorization_play.Test.Static;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace authorization_play.Test
{
    public class PermissionTicketTests
    {
        private static PermissionTicket ExistingTicket => PermissionTicket.Create()
            .ForPrincipal(Identities.DanielB)
            .WithExpiry(DateTimeOffset.MinValue.AddMinutes(1))
            .WithResources(PermissionTicketResource.ForResource(Resources.Farm.Identifier));
        private const string ExistingJwt =
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJpZGVudCI6ImNybjp1c2VyLzQyIiwiZXhwIjo2MjEzNTU5Njc0MCwicmVzb3VyY2UiOlt7InJlc291cmNlIjoiY3JuOmZhcm0vMTIzNCIsInNjaGVtYSI6bnVsbCwiYWN0aW9uIjpbXX1dfQ.ECfKAx-0bHusQLel2Ad-L-VlxfbsehylgrTgD0y2u0tTd1XbeyKr5ogcbakC_DKV949MKWFjNC0FMOEWG5mvHQ";

        [Fact]
        public void ToJwtSuccessful()
        {
            // Act
            var jwt = ExistingTicket.ToJwt(Common.Secret);

            // Assert
            jwt.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void FromJsonSuccessful()
        {
            // Arrange
            var json = @"{
            ""exp"": -62135596740,
            ""ident"": ""crn:user/42"",
            ""resource"": [
            {
                ""resource"": ""crn:farm/1234"",
                ""schema"": null,
                ""action"": []
            }
            ]
        }";

            // Act
            var ticket = JsonConvert.DeserializeObject<PermissionTicket>(json);

            // Assert
            var deserialisedHash = ticket.GetHash();
            var existingHash = ExistingTicket.GetHash();
            deserialisedHash.Should().BeEquivalentTo(existingHash);
        }

        [Fact]
        public void FromJwtSuccessful()
        {
            // Act
            var ticket = PermissionTicket.FromJwt(ExistingJwt, Common.Secret);

            // fix timestamp for hash generation
            ticket.WithExpiry(DateTimeOffset.MinValue.AddMinutes(1));

            // Assert
            var retrievedHash = ticket.GetHash();
            var existingHash = ExistingTicket.GetHash();
            retrievedHash.Should().BeEquivalentTo(existingHash);
        }
    }
}
