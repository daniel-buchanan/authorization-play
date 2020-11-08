using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.DataProviders;
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
        private readonly IDataProviderPolicyApplicator policyApplicator;

        public PermissionValidator(IResourceValidator resourceValidator,
            IResourceFinder resourceFinder,
            IPermissionGrantFinder permissionGrantFinder,
            IDataProviderPolicyApplicator policyApplicator)
        {
            this.resourceValidator = resourceValidator;
            this.resourceFinder = resourceFinder;
            this.permissionGrantFinder = permissionGrantFinder;
            this.policyApplicator = policyApplicator;
        }

        public PermissionValidationResponse Validate(PermissionValidationRequest request)
        {
            var allowedResources = new List<CRN>();
            var deniedResources = new List<CRN>();

            var requestedResources = this.resourceFinder.Find(request.Resource).ToList();
            if (!Validator.TryValidate(() => ValidateResources(requestedResources, request), out var resourceValidationResult))
                return PermissionValidationResponse.InvalidFromResourceValidation(request, resourceValidationResult);

            var issuedGrants = this.permissionGrantFinder.Find(request.Principal, request.Schema);
            foreach (var g in issuedGrants)
            {
                // apply policy at ticket issue time
                if (!this.policyApplicator.IsGrantValid(g)) continue;

                var permissionedResources = this.resourceFinder.Find(g.Resource);
                var intersection = permissionedResources.Intersect(requestedResources);
                var preValidationDeniedResources = requestedResources.Where(r => !intersection.Contains(r)).Select(r => r.Identifier).ToList();
                foreach (var validResource in intersection)
                {
                    Validator.TryValidate(
                        () => this.resourceValidator.Validate(validResource.Identifier, request.Action),
                        out var result);

                    var resourceActionAllowed = g.Actions.Contains(request.Action);
                    if (!resourceActionAllowed || !result.IsValid)
                    {
                        deniedResources.Add(validResource.Identifier);
                        continue;
                    }

                    allowedResources.Add(validResource.Identifier);
                }

                if(preValidationDeniedResources.Any())
                    deniedResources.AddRange(preValidationDeniedResources);
            }

            allowedResources = allowedResources.Distinct().ToList();
            deniedResources = deniedResources.Distinct().ToList();
            return PermissionValidationResponse.From(request, allowedResources, deniedResources);
        }

        private ValidationResult<CRN, ResourceAction, CRN> ValidateResources(IEnumerable<Resource> resources, PermissionRequest request)
        {
            foreach (var r in resources)
            {
                if (!Validator.TryValidate(() => this.resourceValidator.Validate(r.Identifier), out var resultResource))
                    return ValidationResult<CRN, ResourceAction, CRN>.Invalid(r.Identifier, request.Action, request.Principal, resultResource.Reason);

                if (!Validator.TryValidate(() => this.resourceValidator.ValidateAction(request.Action), out var resultAction))
                    return ValidationResult<CRN, ResourceAction, CRN>.Invalid(request.Resource, request.Action, request.Principal, resultAction.Reason);

                if (!Validator.TryValidate(() => this.resourceValidator.Validate(r.Identifier, request.Action), out var resultActionOnResource))
                    return ValidationResult<CRN, ResourceAction, CRN>.Invalid(request.Resource, request.Action, request.Principal, resultActionOnResource.Reason);
            }

            return ValidationResult<CRN, ResourceAction, CRN>.Valid(request.Resource, request.Action, request.Principal);
        }
    }
}
