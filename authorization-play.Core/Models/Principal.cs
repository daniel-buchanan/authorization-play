using System.Collections.Generic;
using authorization_play.Core.Converters;
using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    [JsonConverter(typeof(PrincipalConverter))]
    public class Principal
    {
        public static CRN Platform => CRN.FromValue("crn:internal:platform");

        public CRN Identifier { get; set; }
        
        public List<CRN> Children { get; set; }
    }
}
