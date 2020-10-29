using authorization_play.Core.Models;

namespace authorization_play.Test.Static
{
    public static class Identities
    {
        public static CRN Admin => CRN.FromValue("crn:user/0");
        public static CRN DanielB => CRN.FromValue("crn:user/42");

        public class Organisations
        {
            public static CRN Fonterra => CRN.FromValue("crn:org/666");
            public static CRN OpenCountry => CRN.FromValue("crn:org/667");
        }
    }
}
