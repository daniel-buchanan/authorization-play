using System;
using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;
using authorization_play.Persistance;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Core.Resources
{
    public interface IResourceStorage
    {
        IEnumerable<Resource> All();
        void Add(Resource resource);
        IEnumerable<Resource> FindByIdentifier(CRN identifier);
        void Remove(Resource resource);
    }

    public class ResourceStorage : IResourceStorage
    {
        private readonly AuthorizationPlayContext context;

        public ResourceStorage(AuthorizationPlayContext context)
        {
            this.context = context;
        }

        public void Add(Resource resource)
        {
            var actionNames = resource.ValidActions.Select(a => a.ToString());
            var foundActions = this.context.Actions.Where(a => actionNames.Contains(a.CanonicalName)).ToList();
            var toAdd = new Persistance.Models.Resource()
            {
                CanonicalName = resource.Identifier.ToString()
            };

            var actions = foundActions.Select(a => new Persistance.Models.ResourceAction()
            {
                Resource = toAdd,
                Action = a
            });

            this.context.Resources.Add(toAdd);
            this.context.ResourceActions.AddRange(actions);

            this.context.SaveChanges();
        }

        public IEnumerable<Resource> FindByIdentifier(CRN identifier) => GetQuery().Where(r => r.CanonicalName == identifier).AsEnumerable().Select(ToModel);
        
        public void Remove(Resource resource)
        {
            var existing = this.context.Resources.FirstOrDefault(r => r.CanonicalName == resource.Identifier);
            if (existing == null) return;
            this.context.Remove(existing);
            this.context.SaveChanges();
        }

        public IEnumerable<Resource> All() => GetQuery().AsEnumerable().Select(ToModel);

        private Resource ToModel(Persistance.Models.Resource resource)
        {
            return new Resource()
            {
                Identifier = CRN.FromValue(resource.CanonicalName),
                ValidActions = resource.Actions.Select(a => ResourceAction.FromValue(a.Action.CanonicalName)).ToList()
            };
        }

        private IQueryable<Persistance.Models.Resource> GetQuery() => this.context.Resources.Include(r => r.Actions);
    }
}
