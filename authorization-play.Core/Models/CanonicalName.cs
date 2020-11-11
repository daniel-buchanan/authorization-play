using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    public abstract class CanonicalName
    {
        public static string Wildcard = "*";
        public static string Separator = ":";

        private string[] parts = null;

        [JsonIgnore]
        protected abstract string Prefix { get; }

        public string Value { get; private set; }

        [JsonIgnore]
        public bool IsValid => !string.IsNullOrWhiteSpace(Value) &&
                               Value.StartsWith(Prefix);

        [JsonIgnore]
        public bool IncludesWildcard => Parts.Any(p => p.Contains(Wildcard));

        public IEnumerable<string> Parts
        {
            get
            {
                if (this.parts != null) return this.parts;

                if (string.IsNullOrWhiteSpace(Value)) return new string[0];
                if (!Value.StartsWith(Prefix)) return new string[0];
                var valueParts = Value.Split(Separator);
                if (valueParts.Length <= 1) return new string[0];
                this.parts = valueParts.Skip(1).ToArray();
                return this.parts;
            }
        }

        protected static T FromParts<T>(params string[] parts) where T: CanonicalName, new()
        {
            var instance = new T();
            var combined = string.Join(Separator, parts);
            var rn = $"{instance.Prefix}{Separator}{combined}";
            instance.Value = rn;
            return instance;
        }

        public static T FromValue<T>(string value) where T : CanonicalName, new()
        {
            var instance = new T {Value = value};
            return instance;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value)) return null;
            return !Value.StartsWith(Prefix) ? $"{Prefix}{Separator}{Value}" : Value;
        }

        public override int GetHashCode() => Value?.GetHashCode() ?? -1;

        public bool IsWildcardMatch<T>(T input) where T: CanonicalName
        {
            if (!IncludesWildcard) return false;

            var lastSectionWildcard = parts.Last() == Wildcard;
            var matchExpressions = parts.Select(ConvertPartToRegex).ToList();

            var isMatch = true;
            var inputParts = input.Parts.ToList();

            if (inputParts.Count > this.parts.Length && !lastSectionWildcard) return false;
            if (inputParts.Count < this.parts.Length && !lastSectionWildcard) return false;
            if (inputParts.Count < this.parts.Length - (lastSectionWildcard ? 1 : 0)) return false;

            for (var i = 0; i < matchExpressions.Count; i++)
            {
                if (isMatch == false) break;

                if (parts[i] == Wildcard) break;

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

            if (part == Wildcard) return new Regex(".+");

            part = part.Replace("/", "\\/");
            part = part.Replace(Wildcard, "\\d+");
            return new Regex(part);
        }

        protected bool Equals<T>(T input) where T: CanonicalName => this.ToString() == input?.ToString();
    }
}
