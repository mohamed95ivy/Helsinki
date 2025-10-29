using Helsinki.Application.Interfaces.Providers;
using Helsinki.Infrastructure.Models;
using Helsinki.Infrastructure.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Helsinki.Infrastructure.Providers
{
    // FREE PLAN ON OPENEXCHANGERATES.ORG ONLY SUPPORTS USD AS BASE CURRENCY
    internal class OpenExchangeRatesProvider : IExchangeRateProvider
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey;

        public OpenExchangeRatesProvider(
            HttpClient http,
            IMemoryCache cache,
            IOptions<OpenExchangeRatesOptions> options)
        {
            _http = http;
            _cache = cache;
            _apiKey = options.Value?.ApiKey ?? throw new InvalidOperationException("OpenExchangeRates ApiKey not configured.");

        }
        public async Task<IReadOnlyCollection<string>> GetCurrenciesAsync(CancellationToken ct = default)
        {
            var rates = await GetRatesAsync("USD", ct);
            return rates.Keys.Select(k => k.ToUpperInvariant()).OrderBy(k => k).ToArray();
        }

        public async Task<IDictionary<string, decimal>> GetRatesAsync(string @base, CancellationToken ct = default)
        {
            @base = @base.ToUpperInvariant();
            var cacheKey = $"oxr:rates:{@base}";
            if (_cache.TryGetValue(cacheKey, out IDictionary<string, decimal>? cached))
                return cached!;

            // OXR free plan returns USD-based rates
            var url = $"https://openexchangerates.org/api/latest.json?app_id={_apiKey}";
            var oxr = await _http.GetFromJsonAsync<OpenExchangeRatesResponse>(url, ct)
                      ?? throw new HttpRequestException("Failed to fetch rates from OpenExchangeRates.");

            // Ensure USD present and equals 1
            oxr.Rates["USD"] = 1m;

            var result = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            if (@base == "USD")
            {
                foreach (var (ccy, rate) in oxr.Rates)
                    result[ccy.ToUpperInvariant()] = rate;
            }
            else
            {
                if (!oxr.Rates.TryGetValue(@base, out var usdToBase))
                    throw new KeyNotFoundException($"Unknown base currency '{@base}'.");

                foreach (var (ccy, usdToCcy) in oxr.Rates)
                    result[ccy.ToUpperInvariant()] = usdToCcy / usdToBase;

                result[@base] = 1m;
            }

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return result;
        }
    }
}
