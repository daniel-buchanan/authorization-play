using System.Collections.Generic;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Core.Permissions
{
    public interface IPermissionGrantFinder
    {
        IEnumerable<PermissionGrant> Find(CRN principal, CSN schema);
        IEnumerable<PermissionGrant> Find(CRN principal);
    }
    public class PermissionGrantFinder : IPermissionGrantFinder
    {
        private readonly IPermissionGrantStorage storage;

        public PermissionGrantFinder(IPermissionGrantStorage storage)
        {
            this.storage = storage;
        }

        public IEnumerable<PermissionGrant> Find(CRN principal, CSN schema) =>
            this.storage.GetByPrincipalAndSchema(principal, schema);

        public IEnumerable<PermissionGrant> Find(CRN principal) => this.storage.GetByPrincipal(principal);
    }
}
