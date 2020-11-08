using System.Linq;
using authorization_play.Core.DataProviders;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Principals.Models;
using authorization_play.Core.Resources;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Core.Permissions
{
    public interface IPermissionGrantManager
    {
        bool Add(PermissionGrant grant);
        void Remove(int id);
    }

    public class PermissionGrantManager : IPermissionGrantManager
    {
        private readonly IDataProviderPolicyApplicator applicator;
        private readonly IPermissionGrantStorage storage;
        private readonly IPermissionValidator validator;
        private readonly IResourceStorage resources;

        public PermissionGrantManager(
            IDataProviderPolicyApplicator applicator,
            IPermissionGrantStorage storage,
            IPermissionValidator validator,
            IResourceStorage resources)
        {
            this.applicator = applicator;
            this.storage = storage;
            this.validator = validator;
            this.resources = resources;
        }

        public bool Add(PermissionGrant grant)
        {
            if (!grant.IsValid) return false;

            var allowedByPolicy = this.applicator.IsGrantValid(grant);
            if (!allowedByPolicy) return false;

            if (!ValidateGrantAuthority(grant)) return false;

            this.storage.Add(grant);
            
            return true;
        }

        public void Remove(int id)
        {
            var grant = this.storage.GetById(id);
            this.storage.Remove(grant);
            grant.Id = null;
        }

        private bool ValidateGrantAuthority(PermissionGrant grant)
        {
            if (grant.Grantor == Principal.Platform) return true;

            var allActions = this.resources.AllActions().ToList();
            var ownerAction = allActions.FirstOrDefault(ra => ra == "iam:owner");
            var delegatedAction = allActions.FirstOrDefault(ra => ra == "iam:delegated");

            if (ownerAction == default(ResourceAction) &&
                delegatedAction == default(ResourceAction))
                return false;

            foreach (var resource in grant.Resource)
            {
                var result = this.validator.Validate(new PermissionValidationRequest()
                {
                    Schema = grant.Schema,
                    Principal = grant.Grantor,
                    Action = ownerAction,
                    Resource = resource
                });

                // this is the case for no grants, thus no permissions
                if (result.DeniedResources.Count == 0 && result.AllowedResources.Count == 0) return false;
                if (result.DeniedResources.Contains(resource)) return false;
            }

            return true;
        }
    }
}
