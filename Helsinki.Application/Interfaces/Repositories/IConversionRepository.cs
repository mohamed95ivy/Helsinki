using Helsinki.Domain.Entities;
using static Helsinki.Application.Interfaces.Repositories.IAsyncRepository;

namespace Helsinki.Application.Interfaces.Repositories
{
    /// <summary>
    /// Persists and queries <see cref="ConversionHistory"/> records.
    /// </summary>
    public interface IConversionRepository : IAsyncRepository<ConversionHistory>
    {
        /// <summary>
        /// Returns a paged slice of a user's conversion history.
        /// </summary>
        Task<IReadOnlyList<ConversionHistory>> GetHistoryAsync(
            string userId, int skip, int take, bool newestFirst, CancellationToken ct = default);

        Task<int> GetCountAsync(string userId);
    }
}
