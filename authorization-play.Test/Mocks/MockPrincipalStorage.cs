﻿using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Principals;
using authorization_play.Core.Principals.Models;

namespace authorization_play.Test.Mocks
{
    public class MockPrincipalStorage : IPrincipalStorage
    {
        private readonly List<Principal> storage;

        public MockPrincipalStorage()
        {
            this.storage = new List<Principal>();
        }

        public IEnumerable<Principal> All() => this.storage.AsReadOnly();

        public Principal GetById(int id) => this.storage.FirstOrDefault(p => p.Id == id);

        public IEnumerable<Principal> Find(CPN principal)
        {
            if (principal.IncludesWildcard) 
                return this.storage.Where(p => principal.IsWildcardMatch(p.Identifier));
            
            return this.storage.Where(p => p.Identifier == principal);
        }

        public IEnumerable<Principal> FindParents(CPN principal)
        {
            var parents = new List<Principal>();
            Principal found = this.storage.FirstOrDefault(p => p.Identifier == principal);

            if(found == null) return parents;

            do
            {
                found = this.storage.FirstOrDefault(p => p.Children.Contains(found.Identifier));
                if (found == null) continue;
                parents.Add(found);
            } while (found != null);

            return parents;
        }

        public void Add(Principal principal) => this.storage.Add(principal);

        public bool AddRelation(CPN parent, CPN child)
        {
            var found = this.storage.FirstOrDefault(p => p.Identifier == parent);
            if (found == null) return false;
            found.Children.Add(child);
            return true;
        }

        public void Remove(CPN principal)
        {
            this.storage.RemoveAll(p => p.Identifier == principal);
        }

        public void Remove(int id)
        {
            this.storage.RemoveAll(p => p.Id == id);
        }
    }
}
