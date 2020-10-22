using System.Collections.Generic;
using authorization_play.Core.Converters;
using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    [JsonConverter(typeof(MoASchemaConverter))]
    public class MoASchema
    {
        protected MoASchema(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            IsValid = true;
            Value = value;
        }

        public string Value { get; }
        
        [JsonIgnore]
        public bool IsValid { get; }

        public IEnumerable<string> Parts
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Value)) return new string[0];
                var parts = Value.Split(':');
                if (parts.Length == 0) return new string[0];
                return parts;
            }
        }

        public static MoASchema FromParts(params string[] parts)
        {
            var combined = string.Join(":", parts);
            return new MoASchema(combined);
        }

        public static MoASchema FromValue(string value) => new MoASchema(value);

        public override string ToString() => string.IsNullOrWhiteSpace(Value) ? null : Value;

        public static bool operator ==(MoASchema a, MoASchema b) => a?.ToString() == b?.ToString();

        public static bool operator !=(MoASchema a, MoASchema b) => !(a == b);

        public override int GetHashCode() => Value?.GetHashCode() ?? -1;

        public override bool Equals(object obj) => this.Equals(obj as MoASchema);

        public bool Equals(MoASchema resource) => this == resource;
    }
}
