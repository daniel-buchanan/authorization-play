using authorization_play.Core.Models;

namespace authorization_play.Core.Static
{
    public static class Schemas
    {
        public static MoASchema MilkPickup => MoASchema.FromValue("ag-data:herd:milk-pickup");
        public static MoASchema NumbersOnProperty => MoASchema.FromValue("ag-data:herd:animals:count");
    }
}
