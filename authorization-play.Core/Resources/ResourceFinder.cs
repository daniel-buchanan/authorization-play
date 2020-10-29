using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Core.Resources
{
    public interface IResourceFinder
    {
        IEnumerable<Resource> Find(CRN resource);
        IEnumerable<Resource> Find(IEnumerable<CRN> resources);
    }

    public class ResourceFinder : IResourceFinder
    {
        private readonly IResourceStorage storage;

        public ResourceFinder(IResourceStorage storage)
        {
            this.storage = storage;
        }

        public IEnumerable<Resource> Find(CRN resource)
        {
            var allResources = this.storage.All();
            return Find(resource, allResources);
        }

        private IEnumerable<Resource> Find(CRN resource, IEnumerable<Resource> resources)
        {
            if (resource.IncludesWildcard)
            {
                return resources.Where(r => resource.IsWildcardMatch(r.Identifier));
            }
            return resources.Where(r => r.Identifier == resource);
        }

        public IEnumerable<Resource> Find(IEnumerable<CRN> resources)
        {
            var allResources = this.storage.All().ToList();
            var results = new List<Resource>();
            foreach (var res in resources)
            {
                var found = Find(res, allResources).ToList();
                if(found.Any()) results.AddRange(found);
            }

            return results;
        }
    }
}
