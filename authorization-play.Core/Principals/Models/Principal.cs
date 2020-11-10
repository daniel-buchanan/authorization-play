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
        public static CPN Platform => CPN.FromValue("cpn:internal:platform");

        public int Id { get; set; }

        public CPN Identifier { get; set; }
        
        public List<CPN> Children { get; set; }

        public static Principal FromCrn(CPN identifier) => new Principal()
        {
            Identifier = identifier,
            Children = new List<CPN>()
        };

        public Principal WithChildren(params CPN[] children)
        {
            Children = children?.ToList() ?? new List<CPN>();
            return this;
        }
    }
}
