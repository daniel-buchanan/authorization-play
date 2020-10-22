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
            AllowedResources = new List<MoARN>();
            DeniedResources = new List<MoARN>();
        }

        public PermissionRequest Request { get; set; }
        public List<MoARN> AllowedResources { get; set; }
        public List<MoARN> DeniedResources { get; set; }

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
            PermissionRequest request,
            ValidationResult<MoARN, ResourceAction, MoARN> result) =>
            new PermissionValidationResponse()
            {
                Request = request,
                Reason = result.Reason
            };

        public static PermissionValidationResponse From(
            PermissionRequest request,
            IEnumerable<MoARN> allowed = null,
            IEnumerable<MoARN> denied = null) =>  new PermissionValidationResponse()
        {
            Request = request,
            AllowedResources = allowed?.ToList() ?? new List<MoARN>(),
            DeniedResources = denied?.ToList() ?? new List<MoARN>()
        };
    }
}