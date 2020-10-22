using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Resources.Models;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace authorization_play.Middleware
{
    public class PermissionTicketAuthenticationSchemeOptions
        : AuthenticationSchemeOptions
    {
        
    }

    class PermissionTicketValidationHandler : AuthenticationHandler<PermissionTicketAuthenticationSchemeOptions>
    {
        public PermissionTicketValidationHandler(
            IOptionsMonitor<PermissionTicketAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // validation comes in here
            if (!Request.Headers.ContainsKey("X-Permission-Ticket"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Header Not Found."));
            }

            var token = Request.Headers["X-Permission-Ticket"].ToString();

            var pemTicket = PermissionTicket.FromJwt(token, null);

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

                foreach(var r in pemTicket.Resources)
                    claims.Add(GetClaimForResource(r));

                // generate claimsIdentity on the name of the class
                var claimsIdentity = new ClaimsIdentity(claims,
                            nameof(PermissionTicketValidationHandler));

                // generate AuthenticationTicket from the Identity
                // and current authentication scheme
                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

                // pass on the ticket to the middleware
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Model is Empty"));
        }

        private Claim GetClaimForResource(PermissionTicketResource r)
        {
            var name = $"{r.Identifier}[on]{r.Schema}";
            var actions = string.Join(";", r.Actions);
            return new Claim(name, actions);
        }
    }
}
