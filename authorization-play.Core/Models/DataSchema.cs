using System.Collections.Generic;
using authorization_play.Core.Converters;
using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    [JsonConverter(typeof(DataSchemaConverter))]
    public class DataSchema
    {
        protected DataSchema(string value)
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

        public static DataSchema FromParts(params string[] parts)
        {
            var combined = string.Join(":", parts);
            return new DataSchema(combined);
        }

        public static DataSchema FromValue(string value) => new DataSchema(value);

        public override string ToString() => string.IsNullOrWhiteSpace(Value) ? null : Value;

        public static bool operator ==(DataSchema a, DataSchema b) => a?.ToString() == b?.ToString();

        public static bool operator !=(DataSchema a, DataSchema b) => !(a == b);

        public static bool operator ==(string a, DataSchema b) => a == b?.ToString();

        public static bool operator !=(string a, DataSchema b) => !(a == b);

        public static bool operator ==(DataSchema a, string b) => a?.ToString() == b;

        public static bool operator !=(DataSchema a, string b) => !(a == b);

        public override int GetHashCode() => Value?.GetHashCode() ?? -1;

        public override bool Equals(object obj) => this.Equals(obj as DataSchema);

        public bool Equals(DataSchema resource) => this == resource;
    }
}
