using authorization_play.Core.Models;

namespace authorization_play.Core.DataProviders.Models
{
    public class DataProvider
    {
        public CRN Identifier { get; set; }
        public CRN Principal { get; set; }
        public string Name { get; set; }
    }
}
