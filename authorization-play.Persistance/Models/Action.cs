namespace authorization_play.Persistance.Models
{
    public class Action
    {
        public int ActionId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string CanonicalName { get; set; }
    }
}
