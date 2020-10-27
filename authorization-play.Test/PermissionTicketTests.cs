using System;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Static;
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
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJpZGVudCI6Im1vYXJuOnVzZXIvNDIiLCJleHAiOjYyMTM1NTk2NzQwLCJyZXNvdXJjZSI6W3sicmVzb3VyY2UiOiJtb2FybjpmYXJtLzEyMzQiLCJzY2hlbWEiOm51bGwsImFjdGlvbiI6W119XX0.agFsZsjx2PLBIocJjP0tqIGuN2Q-N6Rl2BHW7Paiep0NBdDEWitgLEpye75egzQOhmlo39RqT0LwhPB5WarA7Q";

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
            ""ident"": ""moarn:user/42"",
            ""resource"": [
            {
                ""resource"": ""moarn:farm/1234"",
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
