using authorization_play.Core.Converters;
using Newtonsoft.Json;

namespace authorization_play.Core.Models
{
    [JsonConverter(typeof(IdValueConverter))]
    public class IdValue
    {
        public IdValue() { }

        protected IdValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            if (string.Equals(value, "*")) Any = true;
            if (int.TryParse(value, out var number)) Value = number;
        }
        public bool Any { get; }
        public int? Value { get; }

        public static IdValue FromValue(string value) => new IdValue(value);
    }
}
