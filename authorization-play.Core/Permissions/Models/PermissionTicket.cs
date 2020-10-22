using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using authorization_play.Core.Converters;
using authorization_play.Core.Models;
using JWT.Algorithms;
using JWT.Builder;
using Newtonsoft.Json;

namespace authorization_play.Core.Permissions.Models
{
    public class PermissionTicket
    {
        private const string HashSecret = "abcdefghijklmnopqrstuvwxyz1234567890";

        public static class Claims
        {
            public const string Identity = "ident";
            public const string Expiry = "exp";
            public const string Resources = "resource";
        }

        public PermissionTicket()
        {
            Resources = new List<PermissionTicketResource>();
        }

        [JsonProperty(Claims.Identity)]
        public MoARN Principal { get; set; }

        [JsonProperty(Claims.Expiry)]
        [JsonConverter(typeof(DateTimeToEpochConverter))]
        public DateTimeOffset Expiry { get; set; }

        [JsonProperty(Claims.Resources)]
        [JsonConverter(typeof(SingleOrArrayConverter<PermissionTicketResource>))]
        public List<PermissionTicketResource> Resources { get; set; }

        [JsonProperty("id")]
        public string Id => GetHash();

        [JsonIgnore]
        public bool IsValid => Principal?.IsValid == true &&
                               Expiry != DateTimeOffset.MinValue &&
                               Resources?.Count > 0 && 
                               Resources?.TrueForAll(r => r.IsValid) == true;

        public static PermissionTicket Invalid() => Create();

        public static PermissionTicket FromValidation(params PermissionValidationResponse[] validations)
        {
            var resources = new List<PermissionTicketResource>();

            var identities = validations.Select(v => v.Request.Principal).Distinct().ToList();
            if (identities.Count() > 1) throw new ArgumentException("All validations *must* be for the same Principal");

            var principal = identities.First();

            foreach (var v in validations)
            {
                if (v.IsValid == PermissionsValid.False) continue;

                foreach (var r in v.AllowedResources)
                {
                    var existing = resources.FirstOrDefault(e => e.Identifier == r &&
                                                                 e.Schema == v.Request.Schema);

                    if (existing == null)
                    {
                        existing = PermissionTicketResource.For(r).ForSchema(v.Request.Schema).WithActions(v.Request.Action);
                        resources.Add(existing);
                    }
                    else
                    {
                        if (existing.Actions.Contains(v.Request.Action)) continue;
                        existing.WithActions(v.Request.Action);
                    }
                }
            }

            return Create().ForPrincipal(principal).WithResources(resources.ToArray());
        }

        public PermissionTicket WithExpiry(DateTimeOffset expiry)
        {
            Expiry = expiry;
            return this;
        }

        public PermissionTicket ForPrincipal(MoARN identity)
        {
            Principal = identity;
            return this;
        }

        public static PermissionTicket Create() => new PermissionTicket();

        public PermissionTicket WithResources(params PermissionTicketResource[] resources)
        {
            if (resources == null || resources.Length == 0) return this;
            if (!Resources.Any()) Resources = resources.ToList();
            else Resources.AddRange(resources);
            return this;
        }

        public bool IsExpired(DateTimeOffset current) => Expiry < current;

        public string ToJwt(string secret)
        {
            if (string.IsNullOrWhiteSpace(secret)) throw new ArgumentNullException(nameof(secret), $"The {nameof(secret)} must be provided to encode the token");

            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA512Algorithm())
                .WithSecret(secret)
                .AddClaim(Claims.Identity, Principal)
                .AddClaim(Claims.Expiry, Expiry.ToUnixTimeSeconds())
                .AddClaim(Claims.Resources, Resources)
                .Encode();
        }

        public static PermissionTicket FromJwt(string jwt, string secret)
        {
            if(string.IsNullOrWhiteSpace(jwt)) throw new ArgumentNullException(nameof(jwt), "The JWT is required");
            if(string.IsNullOrWhiteSpace(secret)) throw new ArgumentNullException(nameof(secret), $"The {nameof(secret)} must be provided to decode the token");

            var json = new JwtBuilder()
                .WithAlgorithm(new HMACSHA512Algorithm())
                .WithSecret(secret)
                .MustVerifySignature()
                .Decode(jwt);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<PermissionTicket>(json);
        }

        public string GetHash()
        {
            var token = ToJwt(HashSecret);
            using (var hash = new SHA512Managed())
            {
                var tokenBytes = Encoding.UTF8.GetBytes(token);
                var hashBytes = hash.ComputeHash(tokenBytes);
                return hashBytes.ToHexString();
            }
        }
    }
}
