using Microsoft.AspNetCore.Mvc;
using System.Linq;
using authorization_play.Core;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Resources.Models;
using Newtonsoft.Json;

namespace authorization_play.Middleware
{
    public static class ControllerExtensions
    {
        public static bool ValidatePermissions<T>(
            this T controller, 
            CRN resource, 
            ResourceAction action, 
            CSN schema)
        where T: ControllerBase
        {
            var principal = controller.User;

            // parse out resources
            var resourceClaims = principal.Claims.Where(c => c.Type.StartsWith("resource"));
            var resourcesAllowed = resourceClaims.Select(c =>
            {
                var base64 = c.Value;
                var json = base64.FromBase64Encoded();
                return JsonConvert.DeserializeObject<PermissionTicketResource>(json);
            });

            // find resources matching schema
            var forSchema = resourcesAllowed.Where(r => r.Schema == schema).ToList();
            if (!forSchema.Any()) return false;
            
            // find resources matching either wildcard or direct match
            var matching = forSchema.Where(r =>
            {
                if (resource.IncludesWildcard) return resource.IsWildcardMatch(r.Identifier);
                return resource == r.Identifier;
            }).ToList();
            if (!matching.Any()) return false;

            // find resources matching required action
            var withAction = matching.Where(r => r.Actions.Contains(action));
            return withAction.Any();
        }
    }
}
