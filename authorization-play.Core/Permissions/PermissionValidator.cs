﻿using System.Collections.Generic;
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

                var grantedResources = this.resourceFinder.Find(g.Resource);
                var intersection = grantedResources.Intersect(requestedResources);
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

        private ValidationResult<CRN, ResourceAction, CPN> ValidateResources(IEnumerable<Resource> resources, PermissionTicketRequest ticketRequest)
        {
            foreach (var r in resources)
            {
                if (!Validator.TryValidate(() => this.resourceValidator.Validate(r.Identifier), out var resultResource))
                    return ValidationResult<CRN, ResourceAction, CPN>.Invalid(r.Identifier, ticketRequest.Action, ticketRequest.Principal, resultResource.Reason);

                if (!Validator.TryValidate(() => this.resourceValidator.ValidateAction(ticketRequest.Action), out var resultAction))
                    return ValidationResult<CRN, ResourceAction, CPN>.Invalid(ticketRequest.Resource, ticketRequest.Action, ticketRequest.Principal, resultAction.Reason);

                if (!Validator.TryValidate(() => this.resourceValidator.Validate(r.Identifier, ticketRequest.Action), out var resultActionOnResource))
                    return ValidationResult<CRN, ResourceAction, CPN>.Invalid(ticketRequest.Resource, ticketRequest.Action, ticketRequest.Principal, resultActionOnResource.Reason);
            }

            return ValidationResult<CRN, ResourceAction, CPN>.Valid(ticketRequest.Resource, ticketRequest.Action, ticketRequest.Principal);
        }
    }
}
