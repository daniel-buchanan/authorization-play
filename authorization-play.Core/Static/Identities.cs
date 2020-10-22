using authorization_play.Core.Models;

namespace authorization_play.Core.Static
{
    public static class Identities
    {
        public static MoARN Admin => MoARN.FromValue("moarn:user/0");
        public static MoARN DanielB => MoARN.FromValue("moarn:user/42");
    }
}
