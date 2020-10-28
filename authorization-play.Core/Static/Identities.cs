using authorization_play.Core.Models;

namespace authorization_play.Core.Static
{
    public static class Identities
    {
        public static CRN Admin => CRN.FromValue("crn:user/0");
        public static CRN DanielB => CRN.FromValue("crn:user/42");
    }
}
