using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Resources;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Core.Permissions
{
    public interface IPermissionValidator
    {
        PermissionValidationResponse Validate(PermissionValidationRequest request);
    }

    public class PermissionValidator : IPermissionValidator
    {
        private readonly IResourceValidator resourceValidator;
        private readonly IResourceFinder resourceFinder;
        private readonly IPermissionGrantFinder permissionGrantFinder;

        public PermissionValidator(IResourceValidator resourceValidator,
            IResourceFinder resourceFinder,
            IPermissionGrantFinder permissionGrantFinder)
        {
            this.resourceValidator = resourceValidator;
            this.resourceFinder = resourceFinder;
            this.permissionGrantFinder = permissionGrantFinder;
        }

        public PermissionValidationResponse Validate(PermissionValidationRequest request)
        {
            var allowedResources = new List<MoARN>();
            var deniedResources = new List<MoARN>();

            var requestedResources = this.resourceFinder.Find(request.Resource).ToList();
            if (!Validator.TryValidate(() => ValidateResources(requestedResources, request), out var resourceValidationResult))
                return PermissionValidationResponse.InvalidFromResourceValidation(request, resourceValidationResult);

            var permissions = this.permissionGrantFinder.Find(request.Principal, request.Schema);
            foreach (var pem in permissions)
            {
                var permissionedResources = this.resourceFinder.Find(pem.Resource);
                var intersection = permissionedResources.Intersect(requestedResources);
                var currentDeniedResources = requestedResources.Where(r => !intersection.Contains(r)).Select(r => r.Identifier).ToList();
                foreach (var validResource in intersection)
                {
                    Validator.TryValidate(
                        () => this.resourceValidator.Validate(validResource.Identifier, request.Action),
                        out var result);

                    if(result.IsValid) allowedResources.Add(validResource.Identifier);
                    else deniedResources.Add(validResource.Identifier);
                }

                if(currentDeniedResources.Any())
                    deniedResources.AddRange(currentDeniedResources);
            }

            allowedResources = allowedResources.Distinct().ToList();
            deniedResources = deniedResources.Distinct().ToList();
            return PermissionValidationResponse.From(request, allowedResources, deniedResources);
        }

        private ValidationResult<MoARN, ResourceAction, MoARN> ValidateResources(IEnumerable<Resource> resources, PermissionRequest request)
        {
            foreach (var r in resources)
            {
                if (!Validator.TryValidate(() => this.resourceValidator.Validate(r.Identifier), out var resultResource))
                    return ValidationResult<MoARN, ResourceAction, MoARN>.Invalid(r.Identifier, request.Action, request.Principal, resultResource.Reason);

                if (!Validator.TryValidate(() => this.resourceValidator.ValidateAction(request.Action), out var resultAction))
                    return ValidationResult<MoARN, ResourceAction, MoARN>.Invalid(request.Resource, request.Action, request.Principal, resultAction.Reason);

                if (!Validator.TryValidate(() => this.resourceValidator.Validate(r.Identifier, request.Action), out var resultActionOnResource))
                    return ValidationResult<MoARN, ResourceAction, MoARN>.Invalid(request.Resource, request.Action, request.Principal, resultActionOnResource.Reason);
            }

            return ValidationResult<MoARN, ResourceAction, MoARN>.Valid(request.Resource, request.Action, request.Principal);
        }
    }
}
