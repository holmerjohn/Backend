using Backend.Domain;

namespace Backend.Data.Repositories
{
    internal class EntityRepository<T> : IEntityRepository<T> where T : Entity
    {
        protected readonly BackendDbContext _dbContext;

        public EntityRepository(BackendDbContext dbContext)
        { 
            _dbContext = dbContext;
        }

        protected virtual async Task<T?> GeyByIdAsync(params object?[]? keyValues)
        {
            if (keyValues == null) return null;

            return await _dbContext.Set<T>().FindAsync(keyValues);
        }

        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
