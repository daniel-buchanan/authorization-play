using authorization_play.Core.Converters;
using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    [JsonConverter(typeof(CanonicalNameConverter<CPN>))]
    public class CPN : CanonicalName
    {
        protected override string Prefix => "cpn";

        public static CPN FromParts(params string[] parts) => FromParts<CPN>(parts);

        public static CPN FromValue(string value) => FromValue<CPN>(value);

        public static implicit operator string(CPN CPN) => CPN?.ToString();

        public static implicit operator CPN(string value) => FromValue(value);

        public static bool operator ==(CPN a, CPN b) => a?.ToString() == b?.ToString();

        public static bool operator !=(CPN a, CPN b) => !(a == b);

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj) => this.Equals(obj as CPN);

        public bool IsWildcardMatch(CPN input) => IsWildcardMatch<CPN>(input);
    }
}
