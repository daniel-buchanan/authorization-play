using authorization_play.Core.Converters;
using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    [JsonConverter(typeof(CanonicalNameConverter<CRN>))]
    public class CRN : CanonicalName
    {
        protected override string Prefix => "crn";

        public static CRN FromParts(params string[] parts) => FromParts<CRN>(parts);

        public static CRN FromValue(string value) => FromValue<CRN>(value);

        public static implicit operator string(CRN crn) => crn?.ToString();

        public static implicit operator CRN(string value) => CRN.FromValue(value);

        public static bool operator ==(CRN a, CRN b) => a?.ToString() == b?.ToString();

        public static bool operator !=(CRN a, CRN b) => !(a == b);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj) => this.Equals(obj as CRN);

        public bool IsWildcardMatch(CRN input) => IsWildcardMatch<CRN>(input);
    }
}
