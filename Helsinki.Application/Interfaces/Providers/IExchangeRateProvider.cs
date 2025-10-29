namespace Helsinki.Application.Interfaces.Providers
{
    /// <summary>
    /// Provides currency exchange rates and available currency codes.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Gets a map of currency codes to their exchange rates relative to the specified base currency.
        /// </summary>
        /// <param name="base">The ISO 4217 base currency code (e.g., "EUR").</param>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>
        /// A dictionary where each key is an ISO 4217 currency code and the value is the rate for 1 <paramref name="base"/> to that currency.
        /// </returns>
        Task<IDictionary<string, decimal>> GetRatesAsync(string @base, CancellationToken ct = default);

        /// <summary>
        /// Gets the list of available ISO 4217 currency codes supported by the provider.
        /// </summary>
        /// <param name="ct">A cancellation token.</param>
        /// <returns>A read-only collection of currency codes.</returns>
        Task<IReadOnlyCollection<string>> GetCurrenciesAsync(CancellationToken ct = default);
    }
}
