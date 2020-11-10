using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Test.Mocks
{
    class MockPermissionGrantStorage : IPermissionGrantStorage
    {
        private readonly List<PermissionGrant> grants;

        public MockPermissionGrantStorage()
        {
            this.grants = new List<PermissionGrant>();
        }

        public void Add(PermissionGrant grant) => this.grants.Add(grant);

        public PermissionGrant GetById(int id) => this.grants.FirstOrDefault(g => g.Id == id);

        public void Remove(PermissionGrant grant) => this.grants.Remove(grant);

        public IEnumerable<PermissionGrant> GetByPrincipal(CPN principal) =>
            this.grants.Where(g => g.Principal == principal);

        public IEnumerable<PermissionGrant> GetByPrincipalAndSchema(CPN principal, CSN schema) =>
            this.grants.Where(g => g.Principal == principal && g.Schema == schema);
    }
}
