using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Helsinki.Application.Interfaces.Repositories.IAsyncRepository;

namespace Helsinki.Infrastructure.Repositories
{
    internal class RepositoryBase<T> : IAsyncRepository<T> where T : class
    {
        protected readonly ApplicationDbContext Db;
        protected readonly DbSet<T> Set;

        public RepositoryBase(ApplicationDbContext db)
        {
            Db = db;
            Set = db.Set<T>();
        }

        public async Task AddAsync(T entity, CancellationToken ct = default)
        {
            Set.Add(entity);
            await Db.SaveChangesAsync(ct);
        }

        public Task<T?> GetByIdAsync(object id, CancellationToken ct = default) => Set.FindAsync(new[] { id }, ct).AsTask();

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
            => await (predicate is null ? Set.AsNoTracking() : Set.AsNoTracking().Where(predicate)).ToListAsync(ct);

    }
}
