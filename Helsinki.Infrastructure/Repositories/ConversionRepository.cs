using Helsinki.Application.Interfaces.Repositories;
using Helsinki.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Helsinki.Infrastructure.Repositories
{
    internal class ConversionRepository : RepositoryBase<ConversionHistory>, IConversionRepository
    {
        public ConversionRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<int> GetCountAsync(string userId)
        {
            return await Db.Conversions
                      .AsNoTracking()
                      .Where(c => c.UserId == userId)
                      .CountAsync();
        }

        public async Task<IReadOnlyList<ConversionHistory>> GetHistoryAsync(string userId, int skip, int take, bool newestFirst, CancellationToken ct = default)
        {
            var q = Db.Conversions.AsNoTracking().Where(x => x.UserId == userId);
            q = newestFirst ? q.OrderByDescending(x => x.ConversionDate) : q.OrderBy(x => x.ConversionDate);
            return await q.Skip(skip).Take(take).ToListAsync(ct);
        }
    }
}
