using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using authorization_play.Core;
using authorization_play.Core.Permissions.Models;
using JWT.Exceptions;
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
        private const string HeaderName = "X-Permission-Ticket";
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
            if (!Request.Headers.ContainsKey(HeaderName)) 
                return AuthenticateResult.Fail("Header Not Found.");

            var token = Request.Headers[HeaderName].ToString();
            if (string.IsNullOrWhiteSpace(token)) 
                return AuthenticateResult.Fail("No Token provided");

            var tokenParts = token.Split(".");
            if (tokenParts.Length != 3) 
                return AuthenticateResult.Fail("Not a valid JWT");

            var pemTicket = GetPermissionTicket(token);
            if(pemTicket == null) 
                return AuthenticateResult.Fail("Not a valid JWT");

            if (await ValidateToken(pemTicket)) 
                return AuthenticateResult.Fail("Not a valid Token");

            var claimsIdentity = GetClaimsIdentity(pemTicket);
            
            // and current authentication scheme
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

            // pass on the ticket to the middleware
            return AuthenticateResult.Success(ticket);
        }

        private ClaimsIdentity GetClaimsIdentity(PermissionTicket pemTicket)
        {
            // create claims array from the model
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, pemTicket.Principal.ToString()),
                new Claim(ClaimTypes.Expiration, pemTicket.Expiry.ToUnixTimeSeconds().ToString())
            };

            AddResourceClaims(claims, pemTicket.Resources);

            // generate claimsIdentity on the name of the class
            return new ClaimsIdentity(claims, nameof(PermissionTicketValidationHandler));
        }

        private PermissionTicket GetPermissionTicket(string token)
        {
            PermissionTicket pemTicket;

            try
            {
                pemTicket = PermissionTicket.FromJwt(token, "secret");
            }
            catch (SignatureVerificationException sigVerifyEx)
            {
                return null;
            }

            return pemTicket;
        }

        private async Task<bool> ValidateToken(PermissionTicket pemTicket)
        {
            using var client = this.httpClientFactory.CreateClient("Test");
            var tokenJson = JsonConvert.SerializeObject(pemTicket);
            var result = await client.PostAsync(new Uri("https://authorization-play.Api/ticket/validate"),
                new StringContent(tokenJson, Encoding.UTF8, "application/json"));
            if (!result.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
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
