using authorization_play.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace authorization_play.Middleware
{
    public class AuthorizePermissionTicketAttribute : AuthorizeAttribute
    {
        public MoARN ResourceIdentifier { get; set; }
        public AuthorizePermissionTicketAttribute()
        {
            AuthenticationSchemes = ServiceCollectionExtensions.SchemeName;
        }

        public AuthorizePermissionTicketAttribute(MoARN resource) : this()
        {
            ResourceIdentifier = resource;
        }
    }
}
