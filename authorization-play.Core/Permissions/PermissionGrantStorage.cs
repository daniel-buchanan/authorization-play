using System;
using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Permissions.Models;

namespace authorization_play.Core.Permissions
{
    public interface IPermissionGrantStorage : IStorage<PermissionGrant> { }

    public class PermissionGrantStorage : IPermissionGrantStorage
    {
        private readonly List<PermissionGrant> grants;

        public PermissionGrantStorage()
        {
            this.grants = new List<PermissionGrant>();
        }

        public void Add(PermissionGrant grant) => this.grants.Add(grant);

        public IEnumerable<PermissionGrant> All() => this.grants.AsReadOnly();

        public PermissionGrant FirstOrDefault(Func<PermissionGrant, bool> predicate) => this.grants.FirstOrDefault(predicate);

        public IEnumerable<PermissionGrant> Where(Func<PermissionGrant, bool> predicate) => this.grants.Where(predicate);

        public void Remove(PermissionGrant grant) => this.grants.Remove(grant);

        public bool Exists(Func<PermissionGrant, bool> predicate) => this.grants.Any(predicate);
    }
}
