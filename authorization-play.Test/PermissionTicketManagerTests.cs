using System;
using System.Collections.Generic;
using authorization_play.Core.DataProviders;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Resources;
using authorization_play.Test.Mocks;
using authorization_play.Test.Static;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace authorization_play.Test
{
    public class PermissionTicketManagerTests
    {
        private readonly IPermissionTicketManager manager;
        private readonly IPermissionTicketStorage storage;

        public PermissionTicketManagerTests()
        {
            var resourceStorage = new MockResourceStorage().Setup();
            var permissionStorage = new MockPermissionGrantStorage().Setup();
            var resourceFinder = new ResourceFinder(resourceStorage);
            var resourceValidator = new ResourceValidator(resourceStorage);
            var permissionFinder = new PermissionGrantFinder(permissionStorage);
            var principalStorage = new MockPrincipalStorage().Setup();
            var dataProviderStorage = new MockDataProviderStorage().Setup();
            var policyApplicator = new DataProviderPolicyApplicator(dataProviderStorage, principalStorage);
            var validator = new PermissionValidator(resourceValidator, resourceFinder, permissionFinder, policyApplicator);
            this.storage = new PermissionTicketStorage();
            this.manager = new PermissionTicketManager(validator, storage);
        }

        [Theory]
        [MemberData(nameof(ValidPermissions))]
        public void ValidTicketIssued(PermissionRequest request)
        {
            // Act
            var ticket = this.manager.Request(request);

            // Assert
            ticket.IsValid.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(ValidPermissions))]
        public void ValidTicketLifetime(PermissionRequest request)
        {
            // Act
            var ticket = this.manager.Request(request);

            // Assert
            ticket.Expiry.Should().BeAfter(DateTimeOffset.UtcNow.AddMinutes(29));
        }

        [Theory]
        [MemberData(nameof(ValidPermissions))]
        public void TicketHasCorrectIdentity(PermissionRequest request)
        {
            // Act
            var ticket = this.manager.Request(request);

            // Assert
            ticket.Principal.Should().Be(request.Principal);
        }

        public static IEnumerable<object[]> ValidPermissions()
        {
            yield return new object[]
            {
                new PermissionRequest()
                {
                    Action = ResourceActions.Data.Read,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756")
                }
            };

            yield return new object[]
            {
                new PermissionRequest()
                {
                    Action = ResourceActions.Data.Read,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756:*")
                }
            };

            yield return new object[]
            {
                new PermissionRequest()
                {
                    Action = ResourceActions.Data.Read,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/1234:herd/88756")
                }
            };

            yield return new object[]
            {
                new PermissionRequest()
                {
                    Action = ResourceActions.Data.Read,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/1234:herd/88756:*")
                }
            };
        }

        [Theory]
        [MemberData(nameof(InvalidPermissions))]
        public void InvalidTickets(PermissionRequest request)
        {
            // Act
            var result = this.manager.Request(request);

            // Assert
            result.IsValid.Should().BeFalse();
        }

        public static IEnumerable<object[]> InvalidPermissions()
        {
            yield return new object[]
            {
                new PermissionRequest()
                {
                    Action = ResourceActions.Iam.Owner,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/8876:*")
                }
            };

            yield return new object[]
            {
                new PermissionRequest()
                {
                    Action = ResourceActions.Iam.Owner,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756")
                }
            };

            yield return new object[]
            {
                new PermissionRequest()
                {
                    Action = ResourceActions.Iam.Delegated,
                    Principal = Identities.DanielB,
                    Schema = Schemas.MilkPickup,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756")
                }
            };

            yield return new object[]
            {
                new PermissionRequest()
                {
                    Action = ResourceActions.Iam.Owner,
                    Principal = Identities.DanielB,
                    Schema = Schemas.NumbersOnProperty,
                    Resource = CRN.FromValue("crn:farm/*:herd/88756")
                }
            };
        }

        [Theory]
        [MemberData(nameof(JsonValidationTests))]
        public void ValidateFromJson(string json, bool expectedResult)
        {
            // Arrange
            if (!string.IsNullOrWhiteSpace(json))
            {
                var ticket = JsonConvert.DeserializeObject<PermissionTicket>(json);
                this.storage.Add(ticket.GetHash(), ticket);
            }

            // Act
            var validationResult = this.manager.Validate(json);

            // Assert
            validationResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> JsonValidationTests()
        {
            yield return new object[] { null, false };
            yield return new object[] { string.Empty, false };
            yield return new object[] { @"{
                ""exp"": -62135596740,
                ""ident"": ""crn:user/42"",
                ""resource"": [
                    {
                        ""resource"": ""crn:farm/1234"",
                        ""schema"": null,
                        ""action"": []
                    }
                ]
            }", false };
            yield return new object[] { @"{
                ""exp"": 62135596740,
                ""ident"": ""crn:user/42"",
                ""resource"": [
                    {
                        ""resource"": ""crn:farm/1234"",
                        ""schema"": ""ag-data:farm:milk-pickup"",
                        ""action"": [
                            ""iam:owner""
                        ]
                    }
                ]
            }", true };
        }

        [Theory]
        [MemberData(nameof(JwtValidationTests))]
        public void ValidateFromJwt(string jwt, bool expectedResult)
        {
            // Arrange
            var ticket = PermissionTicket.FromJwt(jwt, Common.Secret);
            this.storage.Add(ticket.GetHash(), ticket);

            // Act
            var validationResult = this.manager.Validate(jwt, Common.Secret);

            // Assert
            validationResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> JwtValidationTests()
        {
            yield return new object[]
            {
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJpZGVudCI6ImNybjp1c2VyLzQyIiwiZXhwIjo2MjEzNTU5Njc0MCwicmVzb3VyY2UiOlt7InJlc291cmNlIjoiY3JuOmZhcm0vMTIzNCIsInNjaGVtYSI6ImFnLWRhdGE6aGVyZDptaWxrLXBpY2t1cCIsImFjdGlvbiI6WyJpYW06b3duZXIiXX1dfQ.qcmDZhOU0gn0toA5woOZDlilcls6DFU8jqp_XkAspZXSrzp3KijCA6bKHSwCkfLZJK-f_JErnGgRSunaZ1tt6g",
                true
            };

            yield return new object[]
            {
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJpZGVudCI6ImNybjp1c2VyLzQyIiwiZXhwIjo2MjEzNTU5Njc0MCwicmVzb3VyY2UiOlt7InJlc291cmNlIjoiY3JuOmZhcm0vMTIzNCIsInNjaGVtYSI6bnVsbCwiYWN0aW9uIjpbXX1dfQ.ECfKAx-0bHusQLel2Ad-L-VlxfbsehylgrTgD0y2u0tTd1XbeyKr5ogcbakC_DKV949MKWFjNC0FMOEWG5mvHQ",
                false
            };
        }

        [Theory]
        [MemberData(nameof(TicketValidationTests))]
        public void ValidateFromTicket(PermissionTicket ticket, bool expectedResult)
        {
            // Arrange
            this.storage.Add(ticket.GetHash(), ticket);

            // Act
            var validationResult = this.manager.Validate(ticket);

            // Assert
            validationResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> TicketValidationTests()
        {
            yield return new object[]
            {
                PermissionTicket.Create().ForPrincipal(Identities.DanielB)
                    .WithExpiry(DateTimeOffset.UtcNow.AddMinutes(1))
                    .WithResources(PermissionTicketResource.ForResource(Resources.FarmTwo.Identifier).WithActions(ResourceActions.Iam.Owner).ForSchema(Schemas.MilkPickup)),
                true
            };

            yield return new object[]
            {
                PermissionTicket.Create().ForPrincipal(Identities.DanielB)
                    .WithExpiry(DateTimeOffset.UtcNow.AddMinutes(1))
                    .WithResources(PermissionTicketResource.ForResource(Resources.FarmTwo.Identifier).WithActions(ResourceActions.Iam.Owner)),
                false
            };

            yield return new object[]
            {
                PermissionTicket.Create().ForPrincipal(Identities.DanielB)
                    .WithExpiry(DateTimeOffset.UtcNow.AddMinutes(1)),
                false
            };
        }

        [Fact]
        public void TicketIsRevoked()
        {
            // Arrange
            var resourceMask = CRN.FromParts("farm/*", "herd/*");
            var ticket = this.manager.Request(new PermissionRequest()
            {
                Schema = Schemas.MilkPickup,
                Action = ResourceActions.Iam.Owner,
                Principal = Identities.DanielB,
                Resource = resourceMask
            });
            var ticketHash = ticket.GetHash();

            // Act
            this.manager.Revoke(resourceMask, Identities.DanielB);

            // Assert
            var validationResult = this.manager.Validate(ticket);
            var existing = this.storage.Find(ticketHash);
            validationResult.Should().BeFalse();
            existing.Should().BeNull();
        }
    }
}
