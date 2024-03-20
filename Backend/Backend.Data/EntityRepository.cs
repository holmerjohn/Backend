using Backend.Models;

namespace Backend.Data
{
    internal class EntityRepository<T> : IEntityRepository<T> where T : Entity
    {
        private readonly BackendDbContext _dbContext;

        public EntityRepository(BackendDbContext dbContext)
        { 
            _dbContext = dbContext;
        }

        public async Task<T?> GeyByIdAsync(Guid? id)
        {
            if (id == null) return null;

            return await _dbContext.Set<T>().FindAsync(id);
        }
    }
}
