using System;
using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Core.Resources
{
    public interface IResourceStorage : IStorage<Resource> { }

    public class ResourceStorage : IResourceStorage
    {
        private readonly List<Resource> resources;

        public ResourceStorage()
        {
            this.resources = new List<Resource>();
        }

        public void Add(Resource resource) => this.resources.Add(resource);

        public IEnumerable<Resource> All() => this.resources.AsReadOnly();

        public Resource FirstOrDefault(Func<Resource, bool> predicate) => this.resources.FirstOrDefault(predicate);

        public IEnumerable<Resource> Where(Func<Resource, bool> predicate) => this.resources.Where(predicate);

        public void Remove(Resource resource) => this.resources.Remove(resource);

        public bool Exists(Func<Resource, bool> predicate) => this.resources.Any(predicate);
    }
}
