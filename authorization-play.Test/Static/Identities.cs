using authorization_play.Core.Models;
using authorization_play.Core.Principals;
using authorization_play.Core.Principals.Models;

namespace authorization_play.Test.Static
{
    public static class Identities
    {
        public static CPN Platform => Principal.Platform;
        public static CPN Admin => CPN.FromValue("cpn:user/0");
        public static CPN DanielB => CPN.FromValue("cpn:user/42");
        public static CPN Andre => CPN.FromValue("cpn:user/43");

        public class Organisations
        {
            public static CPN Fonterra => CPN.FromValue("cpn:org/666");
            public static CPN OpenCountry => CPN.FromValue("cpn:org/667");
        }

        public static IPrincipalStorage Setup(this IPrincipalStorage storage)
        {
            storage.Add(Principal.From(Admin));
            storage.Add(Principal.From(DanielB));
            storage.Add(Principal.From(Andre));
            storage.Add(Principal.From(Organisations.Fonterra));
            storage.Add(Principal.From(Organisations.OpenCountry));
            return storage;
        }
    }
}
