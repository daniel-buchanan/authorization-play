using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.DataProviders.Models;
using authorization_play.Core.Models;
using authorization_play.Persistance;
using Microsoft.EntityFrameworkCore;

namespace authorization_play.Core.DataProviders
{
    public interface IDataProviderStorage
    {
        void Add(DataProvider provider);
        void AddSource(DataSource source);
        void AddPolicy(DataProviderPolicy policy);
        void Remove(CRN identifier);
        void RemovePolicy(CRN identifier, DataSchema schema);
        IEnumerable<DataProvider> All();
        IEnumerable<DataProviderPolicy> GetPolicies(CRN identifier);
        IEnumerable<DataProviderPolicy> GetPoliciesForSchema(DataSchema schema);
    }

    public class DataProviderStorage : IDataProviderStorage
    {
        private readonly AuthorizationPlayContext context;

        public DataProviderStorage(AuthorizationPlayContext context)
        {
            this.context = context;
        }

        public void Add(DataProvider provider)
        {
            var principal = this.context.Principals.FirstOrDefault(p => p.CanonicalName == provider.Principal.ToString());

            if (principal == null) return;

            var toAdd = new Persistance.Models.DataProvider()
            {
                PrincipalId = principal.PrincipalId,
                CanonicalName = provider.Identifier.ToString(),
                Name = provider.Name
            };

            this.context.Add(toAdd);
            this.context.SaveChanges();
        }

        public void AddSource(DataSource source)
        {
            var provider = this.context.DataProviders.FirstOrDefault(p => p.CanonicalName == source.DataProvider.ToString());

            if (provider == null) return;

            var toAdd = new Persistance.Models.DataSource()
            {
                CanonicalName = source.Identifier.ToString(),
                DataProviderId = provider.DataProviderId
            };

            this.context.Add(toAdd);
            this.context.SaveChanges();
        }

        public void AddPolicy(DataProviderPolicy policy)
        {
            var provider = this.context.DataProviders.FirstOrDefault(p => p.CanonicalName == policy.Provider.ToString());
            var schema = this.context.Schemas.FirstOrDefault(s => s.CanonicalName == policy.Schema.ToString());

            if (provider == null) return;
            if (schema == null) return;

            var toAdd = new Persistance.Models.DataProviderPolicy()
            {
                DataProviderId = provider.DataProviderId,
                SchemaId = schema.SchemaId
            };

            this.context.Add(toAdd);

            var rules = policy.Rule.Select(r => new Persistance.Models.DataProviderPolicyItem()
            {
                Policy = toAdd,
                Principal = this.context.Principals.FirstOrDefault(p => p.CanonicalName == r.Principal.ToString())
            });
            this.context.AddRange(rules);

            this.context.SaveChanges();
        }

        public void Remove(CRN identifier)
        {
            var found = this.context.DataProviders.FirstOrDefault(p => p.CanonicalName == identifier.ToString());
            if (found == null) return;
            this.context.Remove(found);
            this.context.SaveChanges();
        }

        public void RemovePolicy(CRN identifier, DataSchema schema)
        {
            var provider = this.context.DataProviders.FirstOrDefault(p => p.CanonicalName == identifier.ToString());
            var foundSchema = this.context.Schemas.FirstOrDefault(s => s.CanonicalName == schema.ToString());

            if (provider == null || foundSchema == null) return;

            var found = this.context.DataProviderPolicies.FirstOrDefault(p =>
                p.DataProviderId == provider.DataProviderId && p.SchemaId == foundSchema.SchemaId);

            if (found == null) return;
            this.context.Remove(found);
            this.context.SaveChanges();
        }

        public IEnumerable<DataProvider> All()
        {
            return this.context.DataProviders.Include(p => p.Principal).ToList().Select(p => new DataProvider()
            {
                Principal = CRN.FromValue(p.Principal.CanonicalName),
                Identifier = CRN.FromValue(p.CanonicalName),
                Name = p.Name
            });
        }

        public IEnumerable<DataProviderPolicy> GetPolicies(CRN identifier)
        {
            return this.context.DataProviderPolicies
                .Include(p => p.Provider)
                .Include(p => p.Schema)
                .Include(p => p.PolicyItems)
                .ThenInclude(i => i.Principal)
                .Where(p => p.Provider.CanonicalName == identifier.ToString())
                .ToList()
                .Select(p => new DataProviderPolicy()
                {
                    Provider = CRN.FromValue(p.Provider.CanonicalName),
                    Schema = DataSchema.FromValue(p.Schema.CanonicalName),
                    Rule = p.PolicyItems.Select(i => new DataProviderPolicyRule()
                    {
                        Principal = CRN.FromValue(i.Principal.CanonicalName),
                        Allow = i.Allow,
                        Deny = i.Deny
                    }).ToList()
                });
        }

        public IEnumerable<DataProviderPolicy> GetPoliciesForSchema(DataSchema schema)
        {
            return this.context.DataProviderPolicies
                .Include(p => p.Provider)
                .Include(p => p.Schema)
                .Include(p => p.PolicyItems)
                .ThenInclude(i => i.Principal)
                .Where(p => p.Schema.CanonicalName == schema.ToString())
                .ToList()
                .Select(p => new DataProviderPolicy()
                {
                    Provider = CRN.FromValue(p.Provider.CanonicalName),
                    Schema = DataSchema.FromValue(p.Schema.CanonicalName),
                    Rule = p.PolicyItems.Select(i => new DataProviderPolicyRule()
                    {
                        Principal = CRN.FromValue(i.Principal.CanonicalName),
                        Allow = i.Allow,
                        Deny = i.Deny
                    }).ToList()
                });
        }
    }
}
