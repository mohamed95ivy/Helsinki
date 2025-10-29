using Helsinki.Application.Interfaces.Providers;
using Helsinki.Application.Interfaces.Repositories;
using Helsinki.Application.Interfaces.Services;
using Helsinki.Domain.Entities;

namespace Helsinki.Application.Services
{
    internal class ConversionService : IConversionService
    {
        private readonly IRateProviderFactory _factory;
        private readonly IConversionRepository _repo;

        public ConversionService(IRateProviderFactory factory, IConversionRepository repo)
        {
            _factory = factory;
            _repo = repo;
        }
        public async Task<ConversionHistory> ConvertAsync(string userId, string from, string to, decimal amount, CancellationToken ct = default)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

            var provider = _factory.Create();
            var rates = await provider.GetRatesAsync(from.ToUpperInvariant(), ct);

            if (!rates.TryGetValue(to.ToUpperInvariant(), out var rate))
                throw new KeyNotFoundException($"Unknown target currency '{to}'.");

            var toAmount = Math.Round(amount * rate, 6, MidpointRounding.AwayFromZero);

            var record = new ConversionHistory
            {
                UserId = userId,
                FromCurrency = from.ToUpperInvariant(),
                ToCurrency = to.ToUpperInvariant(),
                FromAmount = amount,
                ToAmount = toAmount,
                ExchangeRate = rate,
                ConversionDate = DateTime.UtcNow
            };

            await _repo.AddAsync(record, ct);
            return record;
        }

        public Task<IReadOnlyList<ConversionHistory>> GetHistoryAsync(string userId, int skip, int take, bool newestFirst, CancellationToken ct = default)
            => _repo.GetHistoryAsync(userId, skip, take, newestFirst, ct);

        public async Task<int> GetTotalConversions(string userId)
        {
            return await _repo.GetCountAsync(userId);
        }
    }
}
