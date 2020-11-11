using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Models;
using authorization_play.Persistance;

namespace authorization_play.Core
{
    public interface IDataSchemaStorage
    {
        IEnumerable<DataSchema> All();
        void Add(DataSchema schema);
        void Remove(DataSchema schema);
        void Remove(CSN schema);
    }

    public class DataSchemaStorage : IDataSchemaStorage
    {
        private readonly AuthorizationPlayContext context;

        public DataSchemaStorage(AuthorizationPlayContext context)
        {
            this.context = context;
        }

        public IEnumerable<DataSchema> All()
        {
            return this.context.Schemas.ToList().Select(s => new DataSchema()
            {
                Description = s.Description,
                DisplayName = s.DisplayName,
                Identifier = CSN.FromValue(s.CanonicalName)
            });
        }

        public void Add(DataSchema schema)
        {
            var toAdd = new Persistance.Models.Schema()
            {
                CanonicalName = schema.Identifier.ToString(),
                Description = schema.Description,
                DisplayName = schema.DisplayName
            };

            this.context.Add(toAdd);
            this.context.SaveChanges();
        }

        public void Remove(DataSchema schema)
        {
            Remove(schema?.Identifier);
        }

        public void Remove(CSN schema)
        {
            if (schema == null) return;

            var found = this.context.Schemas.FirstOrDefault(s => s.CanonicalName == schema.ToString());
            if (found == null) return;

            this.context.Remove(found);
            this.context.SaveChanges();
        }
    }
}
