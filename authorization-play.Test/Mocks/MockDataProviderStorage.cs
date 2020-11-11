using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.DataProviders;
using authorization_play.Core.DataProviders.Models;
using authorization_play.Core.Models;

namespace authorization_play.Test.Mocks
{
    public class MockDataProviderStorage : IDataProviderStorage
    {
        private readonly List<DataProvider> providers;
        private readonly List<DataProviderPolicy> policies;
        private readonly List<DataSource> sources;

        public MockDataProviderStorage()
        {
            this.providers = new List<DataProvider>();
            this.policies = new List<DataProviderPolicy>();
            this.sources = new List<DataSource>();
        }

        public void Add(DataProvider provider) => this.providers.Add(provider);

        public void AddSource(DataSource source) => this.sources.Add(source);

        public void AddPolicy(DataProviderPolicy policy) => this.policies.Add(policy);

        public void Remove(CRN identifier) => this.providers.RemoveAll(p => p.Identifier == identifier);

        public void RemovePolicy(CRN identifier, CSN schema) =>
            this.policies.RemoveAll(p => p.Provider == identifier && p.Schema == schema);

        public IEnumerable<DataProvider> All() => this.providers.AsReadOnly();
        public IEnumerable<DataSource> GetSources(CRN identifier) => this.sources.Where(s => s.Provider == identifier);

        public IEnumerable<DataProviderPolicy> GetPolicies(CRN identifier) =>
            this.policies.Where(p => p.Provider == identifier);

        public IEnumerable<DataProviderPolicy> GetPoliciesForSchema(CSN schema) =>
            this.policies.Where(p => p.Schema == schema);
    }
}
