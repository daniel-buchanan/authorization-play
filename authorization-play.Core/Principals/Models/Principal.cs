using System.Collections.Generic;
using System.Linq;
using authorization_play.Core.Converters;
using authorization_play.Core.Models;
using Newtonsoft.Json;

namespace authorization_play.Core.Principals.Models
{
    [JsonConverter(typeof(PrincipalConverter))]
    public class Principal
    {
        public Principal()
        {
            Children = new List<CPN>();
        }

        public static CPN Platform => CPN.FromValue("cpn:internal:platform");

        public int Id { get; private set; }

        public CPN Identifier { get; private set; }
        
        public List<CPN> Children { get; private set; }

        public static Principal From(CPN identifier) => new Principal()
        {
            Identifier = identifier
        };

        public Principal WithId(int id)
        {
            Id = id;
            return this;
        }

        public Principal WithChildren(params CPN[] children)
        {
            Children = children?.ToList() ?? new List<CPN>();
            return this;
        }
    }
}
