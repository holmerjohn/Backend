using Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories
{
    internal class EntityRepository<T> : IEntityRepository<T> where T : Entity
    {
        protected readonly BackendDbContext _dbContext;

        public EntityRepository(BackendDbContext dbContext)
        { 
            _dbContext = dbContext;
        }

        public virtual async Task<T?> GeyByIdAsync(string? identifier, CancellationToken cancellationToken = default)
        {
            if (identifier == null) return null;

            return await _dbContext.Set<T>().SingleOrDefaultAsync(x => x.Id == identifier);
        }

        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
