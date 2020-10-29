using System.Linq;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Core.DataProviders
{
    public interface IDataProviderPolicyApplicator
    {
        bool IsGrantValid(PermissionGrant grant);
    }

    public class DataProviderPolicyApplicator : IDataProviderPolicyApplicator
    {
        private readonly IDataProviderStorage storage;

        public DataProviderPolicyApplicator(IDataProviderStorage storage)
        {
            this.storage = storage;
        }

        public bool IsGrantValid(PermissionGrant grant)
        {
            if (grant.Tag == null || !grant.Tag.Any()) return true;

            var policies = this.storage.GetPoliciesForSchema(grant.Schema).ToList();
            foreach (var tag in grant.Tag)
            {
                var rules = policies
                    .Where(p => p.Provider == tag)
                    .SelectMany(p => p.Rule.Where(r => r.Principal == grant.Principal));
                var denied = rules.Any(r => r.Deny);
                if (denied) return false;
            }

            return true;
        }
    }
}
