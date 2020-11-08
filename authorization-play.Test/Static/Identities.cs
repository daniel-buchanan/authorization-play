using authorization_play.Core.Models;
using authorization_play.Core.Principals;
using authorization_play.Core.Principals.Models;

namespace authorization_play.Test.Static
{
    public static class Identities
    {
        public static CRN Platform => Principal.Platform;
        public static CRN Admin => CRN.FromValue("crn:user/0");
        public static CRN DanielB => CRN.FromValue("crn:user/42");
        public static CRN Andre => CRN.FromValue("crn:user/43");

        public class Organisations
        {
            public static CRN Fonterra => CRN.FromValue("crn:org/666");
            public static CRN OpenCountry => CRN.FromValue("crn:org/667");
        }

        public static IPrincipalStorage Setup(this IPrincipalStorage storage)
        {
            storage.Add(Principal.FromCrn(Admin));
            storage.Add(Principal.FromCrn(DanielB));
            storage.Add(Principal.FromCrn(Andre));
            storage.Add(Principal.FromCrn(Organisations.Fonterra));
            storage.Add(Principal.FromCrn(Organisations.OpenCountry));
            return storage;
        }
    }
}
