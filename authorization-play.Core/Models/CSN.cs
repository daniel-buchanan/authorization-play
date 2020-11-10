using authorization_play.Core.Converters;
using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    [JsonConverter(typeof(CanonicalNameConverter<CSN>))]
    public class CSN : CanonicalName
    {
        protected override string Prefix => "csn";

        public static CSN FromParts(params string[] parts) => FromParts<CSN>(parts);

        public static CSN FromValue(string value) => FromValue<CSN>(value);

        public static implicit operator string(CSN csn) => csn?.ToString();

        public static implicit operator CSN(string value) => FromValue(value);

        public static bool operator ==(CSN a, CSN b) => a?.ToString() == b?.ToString();

        public static bool operator !=(CSN a, CSN b) => !(a == b);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj) => this.Equals(obj as CSN);

        public bool IsWildcardMatch(CSN input) => IsWildcardMatch<CSN>(input);
    }
}
