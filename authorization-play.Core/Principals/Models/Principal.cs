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
        public static CRN Platform => CRN.FromValue("crn:internal:platform");

        public int Id { get; set; }

        public CRN Identifier { get; set; }
        
        public List<CRN> Children { get; set; }

        public static Principal FromCrn(CRN identifier) => new Principal()
        {
            Identifier = identifier,
            Children = new List<CRN>()
        };

        public Principal WithChildren(params CRN[] children)
        {
            Children = children?.ToList() ?? new List<CRN>();
            return this;
        }
    }
}
