using System.Collections.Generic;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Core.Permissions
{
    public interface IPermissionGrantFinder
    {
        IEnumerable<PermissionGrant> Find(MoARN principal, MoASchema schema);
        IEnumerable<PermissionGrant> Find(MoARN principal);
    }
    public class PermissionGrantFinder : IPermissionGrantFinder
    {
        private readonly IPermissionGrantStorage storage;

        public PermissionGrantFinder(IPermissionGrantStorage storage)
        {
            this.storage = storage;
        }

        public IEnumerable<PermissionGrant> Find(MoARN principal, MoASchema schema)
        {
            return this.storage.Where(p => p.Principal == principal && p.Schema == schema);
        }

        public IEnumerable<PermissionGrant> Find(MoARN principal)
        {
            return this.storage.Where(p => p.Principal == principal);
        }
    }
}
