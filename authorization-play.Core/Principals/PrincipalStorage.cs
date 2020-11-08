using System;
using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Principals.Models;
using authorization_play.Persistance;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Core.Principals
{
    public interface IPrincipalStorage
    {
        IEnumerable<Principal> All();
        Principal GetById(int id);
        IEnumerable<Principal> Find(CRN principal);
        IEnumerable<Principal> FindParents(CRN principal);
        void Add(Principal principal);
    }

    public class PrincipalStorage : IPrincipalStorage
    {
        private readonly AuthorizationPlayContext context;

        public PrincipalStorage(AuthorizationPlayContext context)
        {
            this.context = context;
        }

        public IEnumerable<Principal> All() => GetQuery().ToList().Select(ToModel);

        public Principal GetById(int id)
        {
            var found = GetQuery().FirstOrDefault(p => p.PrincipalId == id);
            return ToModel(found);
        }

        public IEnumerable<Principal> Find(CRN principal)
        {
            if (principal.IncludesWildcard) return FindByWildcard(principal);
            return GetQuery()
                .Where(p => p.CanonicalName == principal.Value)
                .ToList()
                .Select(ToModel);
        }

        private IEnumerable<Principal> FindByWildcard(CRN principal)
        {
            var all = GetQuery().ToList();
            return all
                .Where(p => principal.IsWildcardMatch(CRN.FromValue(p.CanonicalName)))
                .Select(ToModel);
        }

        public IEnumerable<Principal> FindParents(CRN principal)
        {
            if(principal.IncludesWildcard) 
                throw new ArgumentException("Parentage search for principal cannot include wildcards");

            var found = GetQuery().FirstOrDefault(p => p.CanonicalName == principal.Value);

            if (found == null) return new List<Principal>();

            var parents = new List<Principal>();
            Persistance.Models.PrincipalRelation link = null;

            do
            {
                link = this.context.PrincipalRelations
                    .Include(r => r.Parent)
                    .FirstOrDefault(r => r.ChildPrincipalId == found.PrincipalId);

                if (link == null) continue;

                parents.Add(ToModel(link.Parent));
                found = link.Parent;
            } while (link != null);

            return parents;
        }

        public void Add(Principal principal)
        {
            var toAdd = new Persistance.Models.Principal()
            {
                CanonicalName = principal.Identifier
            };

            this.context.Add(toAdd);

            foreach (var child in principal.Children)
            {
                var found = this.context.Principals.FirstOrDefault(p => p.CanonicalName == child.Value);
                if (found == null) continue;
                this.context.Add(new Persistance.Models.PrincipalRelation()
                {
                    Parent = toAdd,
                    Child = found
                });
            }

            this.context.SaveChanges();
        }

        private Principal ToModel(Persistance.Models.Principal entity)
        {
            if (entity == null) return null;
            return new Principal()
            {
                Identifier = CRN.FromValue(entity.CanonicalName),
                Children = entity.ChildRelations.Select(r => CRN.FromValue(r.Child.CanonicalName)).ToList()
            };
        }

        private IQueryable<Persistance.Models.Principal> GetQuery() =>
            this.context.Principals
                .Include(p => p.ChildRelations)
                .ThenInclude(r => r.Child);
    }
}
