using System.Collections.Generic;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Core.Permissions
{
    public interface IPermissionGrantFinder
    {
        IEnumerable<PermissionGrant> Find(CPN principal, CSN schema);
        IEnumerable<PermissionGrant> Find(CPN principal);
    }
    public class PermissionGrantFinder : IPermissionGrantFinder
    {
        private readonly IPermissionGrantStorage storage;

        public PermissionGrantFinder(IPermissionGrantStorage storage)
        {
            this.storage = storage;
        }

        public IEnumerable<PermissionGrant> Find(CPN principal, CSN schema) =>
            this.storage.GetByPrincipalAndSchema(principal, schema);

        public IEnumerable<PermissionGrant> Find(CPN principal) => this.storage.GetByPrincipal(principal);
    }
}
