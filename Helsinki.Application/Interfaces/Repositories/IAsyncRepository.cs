using System.Linq.Expressions;

namespace Helsinki.Application.Interfaces.Repositories
{
    public interface IAsyncRepository
    {
        /// <summary>
        /// Generic async repository for aggregate roots/entities.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        public interface IAsyncRepository<T> where T : class
        {
            /// <summary>Adds an entity and persists changes.</summary>
            Task AddAsync(T entity, CancellationToken ct = default);

            /// <summary>Gets an entity by its primary key, or null if not found.</summary>
            Task<T?> GetByIdAsync(object id, CancellationToken ct = default);

            /// <summary>Returns all entities that satisfy a predicate (or all if null).</summary>
            Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
        }
    }
}
