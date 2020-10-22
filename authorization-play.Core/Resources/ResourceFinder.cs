using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Core.Resources
{
    public interface IResourceFinder
    {
        IEnumerable<Resource> Find(MoARN resource);
        IEnumerable<Resource> Find(IEnumerable<MoARN> resources);
    }

    public class ResourceFinder : IResourceFinder
    {
        private readonly IResourceStorage storage;

        public ResourceFinder(IResourceStorage storage)
        {
            this.storage = storage;
        }

        public IEnumerable<Resource> Find(MoARN resource)
        {
            if (resource.IncludesWildcard) return FindByWildCard(resource);
            return this.storage.Where(r => r.Identifier == resource);
        }

        public IEnumerable<Resource> Find(IEnumerable<MoARN> resources)
        {
            var results = new List<Resource>();
            foreach (var res in resources)
            {
                var found = Find(res).ToList();
                if(found.Any()) results.AddRange(found);
            }

            return results;
        }

        private IEnumerable<Resource> FindByWildCard(MoARN input)
        {
            if (input.IncludesWildcard)
            {
                return this.storage.Where(r => input.IsWildcardMatch(r.Identifier));
            }

            return this.storage.Where(r => r.Identifier == input);
        }
    }
}
