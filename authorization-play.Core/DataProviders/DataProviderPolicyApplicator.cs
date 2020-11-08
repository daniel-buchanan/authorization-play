using System.Linq;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Principals;

namespace authorization_play.Core.DataProviders
{
    public interface IDataProviderPolicyApplicator
    {
        bool IsGrantValid(PermissionGrant grant);
    }

    public class DataProviderPolicyApplicator : IDataProviderPolicyApplicator
    {
        private readonly IDataProviderStorage storage;
        private readonly IPrincipalStorage principalStorage;

        public DataProviderPolicyApplicator(IDataProviderStorage storage,
            IPrincipalStorage principalStorage)
        {
            this.storage = storage;
            this.principalStorage = principalStorage;
        }

        public bool IsGrantValid(PermissionGrant grant)
        {
            if (grant.Tag == null || !grant.Tag.Any()) return true;
            var parents = this.principalStorage.FindParents(grant.Principal).Select(p => p.Identifier);

            var policies = this.storage.GetPoliciesForSchema(grant.Schema).ToList();
            foreach (var tag in grant.Tag)
            {
                var rules = policies
                    .Where(p => p.Provider == tag)
                    .SelectMany(p => p.Rule.Where(r => 
                        r.Principal == grant.Principal ||
                        parents.Contains(r.Principal)));
                var denied = rules.Any(r => r.Deny);
                if (denied) return false;
            }

            return true;
        }
    }
}
