namespace authorization_play.Persistance.Models
{
    public class DataSource
    {
        public int DataSourceId { get; set; }
        public int DataProviderId { get; set; }
        public DataProvider Provider { get; set; }
        public string CanonicalName { get; set; }
    }
}
