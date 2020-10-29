using authorization_play.Core.DataProviders;
using authorization_play.Core.Permissions.Models;

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

        public PermissionGrantManager(
            IDataProviderPolicyApplicator applicator,
            IPermissionGrantStorage storage)
        {
            this.applicator = applicator;
            this.storage = storage;
        }

        public bool Add(PermissionGrant grant)
        {
            if (!grant.IsValid) return false;
            var allowedByPolicy = this.applicator.GrantValid(grant);
            if(allowedByPolicy) this.storage.Add(grant);
            return allowedByPolicy;
        }

        public void Remove(int id)
        {
            var grant = this.storage.GetById(id);
            this.storage.Remove(grant);
            grant.Id = null;
        }
    }
}
