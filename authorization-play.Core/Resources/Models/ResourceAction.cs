using authorization_play.Core.Converters;
using Newtonsoft.Json;

namespace authorization_play.Core.Resources.Models
{
    [JsonConverter(typeof(ResourceActionConverter))]
    public class ResourceAction
    {
        private readonly string value;

        public ResourceAction() { }

        protected ResourceAction(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            var parts = value.Split(':');
            if (parts.Length == 0) return;
            if (parts.Length == 1) Category = parts[0];
            else
            {
                Category = parts[0];
                Action = parts[1];
            }

            if (string.IsNullOrWhiteSpace(Category) &&
                string.IsNullOrWhiteSpace(Action))
                this.value = null;
            else if (string.IsNullOrWhiteSpace(Category) && !string.IsNullOrWhiteSpace(Action))
                this.value = $"na:{Action}";
            else if (!string.IsNullOrWhiteSpace(Category) && string.IsNullOrWhiteSpace(Action))
                this.value = Category;
            else this.value = $"{Category}:{Action}";
        }

        public string Category { get; }

        public string Action { get; }

        public override string ToString() => this.value;

        public static bool operator ==(ResourceAction a, ResourceAction b) => a?.ToString() == b?.ToString();

        public static bool operator !=(ResourceAction a, ResourceAction b) => !(a == b);

        public override int GetHashCode() => this.value?.GetHashCode() ?? -1;

        public override bool Equals(object obj) => this.Equals(obj as ResourceAction);

        public bool Equals(ResourceAction resource) => this == resource;

        public static ResourceAction FromValue(string value) => new ResourceAction(value);
    }
}
