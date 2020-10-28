using authorization_play.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace authorization_play.Middleware
{
    public class AuthorizePermissionTicketAttribute : AuthorizeAttribute
    {
        public CRN ResourceIdentifier { get; set; }
        public AuthorizePermissionTicketAttribute()
        {
            AuthenticationSchemes = ServiceCollectionExtensions.SchemeName;
        }

        public AuthorizePermissionTicketAttribute(CRN resource) : this()
        {
            ResourceIdentifier = resource;
        }
    }
}
