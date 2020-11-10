using authorization_play.Core.Models;

namespace authorization_play.Test.Static
{
    public static class Schemas
    {
        public static CSN MilkPickup => CSN.FromValue("csn:ag-data:herd:milk-pickup");
        public static CSN NumbersOnProperty => CSN.FromValue("csn:ag-data:herd:animals:count");
    }
}
