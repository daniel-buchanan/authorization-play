using System;
using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Permissions.Models;
using authorization_play.Core.Resources.Models;
using authorization_play.Persistance;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Core.Permissions
{
    public interface IPermissionGrantStorage
    {
        void Add(PermissionGrant grant);
        PermissionGrant GetById(int id);
        void Remove(PermissionGrant grant);
        IEnumerable<PermissionGrant> GetByPrincipal(CRN principal);
        IEnumerable<PermissionGrant> GetByPrincipalAndSchema(CRN principal, DataSchema schema);
    }

    public class PermissionGrantStorage : IPermissionGrantStorage
    {
        private readonly AuthorizationPlayContext context;

        public PermissionGrantStorage(AuthorizationPlayContext context)
        {
            this.context = context;
        }

        public void Add(PermissionGrant grant)
        {
            var principal = this.context.Principals.FirstOrDefault(p => p.CanonicalName == grant.Principal);
            var schema = this.context.Schemas.FirstOrDefault(s => s.CanonicalName == grant.Schema);
            var actions = this.context.Actions.Where(a => grant.Actions.Contains(ResourceAction.FromValue(a.CanonicalName)));
            var resources = this.context.Resources.Where(r => grant.Resource.Contains(CRN.FromValue(r.CanonicalName)));

            var toAdd = new Persistance.Models.PermissionGrant()
            {
                Principal = principal,
                Schema = schema
            };

            var actionsToAdd = actions.Select(a => new Persistance.Models.PermissionGrantResourceAction()
            {
                Action = a,
                Grant = toAdd
            });

            var resourcesToAdd = resources.Select(r => new Persistance.Models.PermissionGrantResource()
            {
                Resource = r,
                Grant = toAdd
            });

            this.context.PermissionGrants.Add(toAdd);
            this.context.PermissionGrantResourceActions.AddRange(actionsToAdd);
            this.context.PermissionGrantResources.AddRange(resourcesToAdd);

            this.context.SaveChanges();
        }

        public PermissionGrant GetById(int id)
        {
            var query = GetQuery();
            return ToModel(query.FirstOrDefault(g => g.PermissionGrantId == id));
        }

        public void Remove(PermissionGrant grant)
        {
            var found = this.context.PermissionGrants.FirstOrDefault(g => g.PermissionGrantId == grant.Id);
            if (found == null) return;
            this.context.PermissionGrants.Remove(found);
            this.context.SaveChanges();
        }

        public IEnumerable<PermissionGrant> GetByPrincipal(CRN principal) => GetByPrincipalAndSchema(principal, null);

        public IEnumerable<PermissionGrant> GetByPrincipalAndSchema(CRN principal, DataSchema schema)
        {
            var foundPrincipal = this.context.Principals.FirstOrDefault(p => p.CanonicalName == principal);
            var foundSchema = this.context.Schemas.FirstOrDefault(s => s.CanonicalName == schema);
            if (foundPrincipal == null) return new List<PermissionGrant>();
            var query = GetQuery();
            Func<Persistance.Models.PermissionGrant, bool> filter;
            if (foundSchema != null)
                filter = (g) => g.PrincipalId == foundPrincipal.PrincipalId && g.SchemaId == foundSchema.SchemaId;
            else
                filter = g => g.PrincipalId == foundPrincipal.PrincipalId;

            return query.Where(filter).Select(ToModel);
        }

        private PermissionGrant ToModel(Persistance.Models.PermissionGrant entity)
        {
            return new PermissionGrant()
            {
                Id = entity.PermissionGrantId,
                Principal = CRN.FromValue(entity.Principal.CanonicalName),
                Schema = DataSchema.FromValue(entity.Schema.CanonicalName),
                Actions = entity.Actions.Select(a => ResourceAction.FromValue(a.Action.CanonicalName)).ToList(),
                Resource = entity.Resources.Select(r => CRN.FromValue(r.Resource.CanonicalName)).ToList()
            };
        }

        private IQueryable<Persistance.Models.PermissionGrant> GetQuery() => this.context.PermissionGrants
            .Include(p => p.Actions)
            .ThenInclude(p => p.Action)
            .Include(p => p.Schema)
            .Include(p => p.Principal)
            .Include(p => p.Resources)
            .ThenInclude(p => p.Resource)
            .Include(p => p.Tags)
            .ThenInclude(p => p.Tag);
    }
}
