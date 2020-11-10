using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Core.Permissions.Models
{
    public class PermissionValidationResponse
    {
        public PermissionValidationResponse()
        {
            AllowedResources = new List<CRN>();
            DeniedResources = new List<CRN>();
        }

        public PermissionTicketRequest TicketRequest { get; set; }
        public List<CRN> AllowedResources { get; set; }
        public List<CRN> DeniedResources { get; set; }

        public PermissionsValid IsValid
        {
            get
            {
                if (AllowedResources.Any() && DeniedResources.Any())
                    return PermissionsValid.Partial;
                if (AllowedResources.Any() && !DeniedResources.Any())
                    return PermissionsValid.True;
                if (!AllowedResources.Any() && DeniedResources.Any())
                    return PermissionsValid.False;
                return PermissionsValid.False;
            }
        }

        public string Reason { get; set; }

        public static PermissionValidationResponse InvalidFromResourceValidation(
            PermissionTicketRequest ticketRequest,
            ValidationResult<CRN, ResourceAction, CPN> result) =>
            new PermissionValidationResponse()
            {
                TicketRequest = ticketRequest,
                Reason = result.Reason
            };

        public static PermissionValidationResponse From(
            PermissionTicketRequest ticketRequest,
            IEnumerable<CRN> allowed = null,
            IEnumerable<CRN> denied = null) =>  new PermissionValidationResponse()
        {
            TicketRequest = ticketRequest,
            AllowedResources = allowed?.ToList() ?? new List<CRN>(),
            DeniedResources = denied?.ToList() ?? new List<CRN>()
        };
    }
}