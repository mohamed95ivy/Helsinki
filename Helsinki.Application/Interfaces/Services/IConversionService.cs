using Helsinki.Domain.Entities;

namespace Helsinki.Application.Interfaces.Services
{
    public interface IConversionService
    {
        Task<ConversionHistory> ConvertAsync(
            string userId, string from, string to, decimal amount, CancellationToken ct = default);

        Task<IReadOnlyList<ConversionHistory>> GetHistoryAsync(
            string userId, int skip, int take, bool newestFirst, CancellationToken ct = default);

        Task<int> GetTotalConversions(string userId);
    }
}
