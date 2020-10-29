using authorization_play.Core.Models;

namespace authorization_play.Core.DataProviders.Models
{
    public class DataProvider
    {
        public CRN Identifier { get; set; }
        public CRN Principal { get; set; }
        public string Name { get; set; }

        public static DataProvider FromIdentifier(CRN identifier) => new DataProvider()
        {
            Identifier = identifier
        };

        public DataProvider ForPrincipal(CRN principal)
        {
            Principal = principal;
            return this;
        }

        public DataProvider WithName(string name)
        {
            Name = name;
            return this;
        }
    }
}
