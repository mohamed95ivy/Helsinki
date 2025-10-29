using System.Text.Json.Serialization;

namespace Helsinki.Infrastructure.Models
{
    public class OpenExchangeRatesResponse
    {
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; } = default!;

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    }
}
