using System;
using Microsoft.AspNetCore.Authentication;

namespace authorization_play.Middleware
{
    public static class ServiceCollectionExtensions
    {
        public const string SchemeName = "PemTicket";

        public static AuthenticationBuilder AddPermissionTicketAuthorization(this AuthenticationBuilder builder) =>
            AddPermissionTicketAuthorization(builder, options => { });

        public static AuthenticationBuilder AddPermissionTicketAuthorization(this AuthenticationBuilder builder,
            Action<PermissionTicketAuthenticationSchemeOptions> configureOptions)
        {
            builder.AddScheme<PermissionTicketAuthenticationSchemeOptions, PermissionTicketValidationHandler>(
                SchemeName, configureOptions);
            return builder;
        }
    }
}
