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
            if (resource.IncludesWildcard)
            {
                return this.storage.All().Where(r => resource.IsWildcardMatch(r.Identifier));
            }
            return this.storage.FindByIdentifier(resource);
        }

        public IEnumerable<Resource> Find(IEnumerable<CRN> resources)
        {
            var results = new List<Resource>();
            foreach (var res in resources)
            {
                var found = Find(res).ToList();
                if(found.Any()) results.AddRange(found);
            }

            return results;
        }
    }
}
