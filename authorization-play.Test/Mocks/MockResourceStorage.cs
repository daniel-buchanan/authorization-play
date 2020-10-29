using System;
using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Core.Resources;
using authorization_play.Core.Resources.Models;

namespace authorization_play.Test.Mocks
{
    public class MockResourceStorage : IResourceStorage
    {
        private readonly List<Resource> resources;
        private readonly List<ResourceAction> actions;

        public MockResourceStorage()
        {
            this.resources = new List<Resource>();
            this.actions = new List<ResourceAction>();
        }

        public void Add(Resource resource) => this.resources.Add(resource);

        public void AddAction(ResourceAction action) => this.actions.Add(action);

        public IEnumerable<ResourceAction> AllActions() => this.actions.AsReadOnly();

        public IEnumerable<Resource> FindByIdentifier(CRN identifier) => this.resources.Where(r => r.Identifier == identifier);

        public IEnumerable<Resource> All() => this.resources.AsReadOnly();

        public Resource FirstOrDefault(Func<Resource, bool> predicate) => this.resources.FirstOrDefault(predicate);

        public IEnumerable<Resource> Where(Func<Resource, bool> predicate) => this.resources.Where(predicate);

        public void Remove(Resource resource) => this.resources.Remove(resource);

        public bool Exists(Func<Resource, bool> predicate) => this.resources.Any(predicate);
    }
}
