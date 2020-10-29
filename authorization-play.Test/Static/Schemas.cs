using authorization_play.Core.Models;

namespace authorization_play.Test.Static
{
    public static class Schemas
    {
        public static DataSchema MilkPickup => DataSchema.FromValue("ag-data:herd:milk-pickup");
        public static DataSchema NumbersOnProperty => DataSchema.FromValue("ag-data:herd:animals:count");
    }
}
