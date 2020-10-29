using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using authorization_play.Core;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Resources.Models;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace authorization_play.Middleware
{
    public class PermissionTicketAuthenticationSchemeOptions
        : AuthenticationSchemeOptions
    {
        
    }

    class PermissionTicketValidationHandler : AuthenticationHandler<PermissionTicketAuthenticationSchemeOptions>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PermissionTicketValidationHandler(
            IOptionsMonitor<PermissionTicketAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IHttpClientFactory httpClientFactory) 
            : base(options, logger, encoder, clock)
        {
            this.httpClientFactory = httpClientFactory;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // validation comes in here
            if (!Request.Headers.ContainsKey("X-Permission-Ticket"))
            {
                return AuthenticateResult.Fail("Header Not Found.");
            }

            var token = Request.Headers["X-Permission-Ticket"].ToString();

            if (string.IsNullOrWhiteSpace(token)) return AuthenticateResult.Fail("No Token provided");
            var tokenParts = token.Split(".");
            if (tokenParts.Length != 3) return AuthenticateResult.Fail("Not a valid JWT");

            PermissionTicket pemTicket;

            try
            {
                pemTicket = PermissionTicket.FromJwt(token, "secret");
            }
            catch (JWT.Exceptions.SignatureVerificationException sigVerifyEx)
            {
                return AuthenticateResult.Fail("Not a valid JWT");
            }

            using var client = this.httpClientFactory.CreateClient("Test");
            var tokenJson = JsonConvert.SerializeObject(pemTicket);
            var result = await client.PostAsync(new Uri("https://authorization-play.Api/ticket/validate"), new StringContent(tokenJson, Encoding.UTF8, "application/json"));
            if(!result.IsSuccessStatusCode) return AuthenticateResult.Fail("Invalid Ticket");

            if (pemTicket != null)
            {
                // success case AuthenticationTicket generation
                // happens from here

                // create claims array from the model
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, pemTicket.Principal.ToString()),
                    new Claim(ClaimTypes.Expiration, pemTicket.Expiry.ToUnixTimeSeconds().ToString())
                };

                AddResourceClaims(claims, pemTicket.Resources);

                // generate claimsIdentity on the name of the class
                var claimsIdentity = new ClaimsIdentity(claims,
                            nameof(PermissionTicketValidationHandler));

                // generate AuthenticationTicket from the Identity
                // and current authentication scheme
                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

                // pass on the ticket to the middleware
                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Model is Empty");
        }

        private void AddResourceClaims(List<Claim> claims, List<PermissionTicketResource> resources)
        {
            for (var i = 0; i < resources.Count; i++)
            {
                var json = JsonConvert.SerializeObject(resources[i]);
                var base64Encoded = json.ToBase64Encoded();
                var claimName = $"resource[{i}]";
                claims.Add(new Claim(claimName, base64Encoded));
            }
        }
    }
}
