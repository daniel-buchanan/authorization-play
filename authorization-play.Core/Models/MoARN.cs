using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using authorization_play.Core.Converters;
using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    [JsonConverter(typeof(MoARNConverter))]
    public class MoARN
    {
        private string[] parts = null;

        public MoARN() { }

        protected MoARN(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            if (!value.StartsWith("moarn")) return;

            IsValid = true;
            Value = value;
        }

        public string Value { get; }

        [JsonIgnore]
        public bool IsValid { get; }

        public bool IncludesWildcard => Parts.Any(p => p.Contains("*"));

        public IEnumerable<string> Parts
        {
            get
            {
                if (this.parts != null) return this.parts;

                if(string.IsNullOrWhiteSpace(Value)) return new string[0];
                if(!Value.StartsWith("moarn")) return new string[0];
                var valueParts = Value.Split(':');
                if(valueParts.Length <= 1) return new string[0];
                this.parts = valueParts.Skip(1).ToArray();
                return this.parts;
            }
        }

        public static MoARN FromParts(params string[] parts)
        {
            var combined = string.Join(":", parts);
            var rn = $"moarn:{combined}";
            return new MoARN(rn);
        }

        public static MoARN FromValue(string value) => new MoARN(value);

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return null;
            if (!Value.StartsWith("moarn:")) return $"moarn:{Value}";
            return Value;
        }

        public IdValue GetIdValue(string prefix)
        {
            var found = Parts.FirstOrDefault(p => p.StartsWith(prefix));
            if (found == null) return new IdValue();
            var parts = found.Split('/');
            var value = parts[1];
            return IdValue.FromValue(value);
        }

        public static bool operator ==(MoARN a, MoARN b) => a?.ToString() == b?.ToString();

        public static bool operator !=(MoARN a, MoARN b) => !(a == b);

        public override int GetHashCode() => Value?.GetHashCode() ?? -1;

        public override bool Equals(object obj) => this.Equals(obj as MoARN);

        public bool Equals(MoARN resource) => this == resource;

        public bool IsWildcardMatch(MoARN input)
        {
            if (!IncludesWildcard) return false;

            var lastSectionWildcard = parts.Last() == "*";
            var matchExpressions = parts.Select(ConvertPartToRegex).ToList();

            var isMatch = true;
            var inputParts = input.Parts.ToList();

            if (inputParts.Count > this.parts.Length && !lastSectionWildcard) return false;
            if (inputParts.Count < this.parts.Length && !lastSectionWildcard) return false;
            if (inputParts.Count < this.parts.Length - (lastSectionWildcard ? 1 : 0)) return false;

            for (var i = 0; i < matchExpressions.Count; i++)
            {
                if (isMatch == false) break;

                if (parts[i] == "*")
                {
                    isMatch = true;
                    break;
                }

                if (i >= inputParts.Count)
                {
                    isMatch = false;
                    break;
                }

                isMatch &= matchExpressions[i].IsMatch(inputParts[i]);
            }

            return isMatch;
        }

        private static Regex ConvertPartToRegex(string part)
        {
            if (string.IsNullOrWhiteSpace(part)) return null;

            if (part == "*") return new Regex(".+");

            part = part.Replace("/", "\\/");
            part = part.Replace("*", "\\d+");
            return new Regex(part);
        }
    }
}
