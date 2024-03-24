using Backend.Domain.Facts;
using Backend.Enums;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories
{
    internal class FactStatusRepository : EntityRepository<FactStatus>, IFactStatusRepository
    {
        public FactStatusRepository(BackendDbContext factStatusRepository)
            : base(factStatusRepository)
        { }

        /// <summary>
        /// Get or create a new FactStatus object
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="enitityId"></param>
        /// <param name="factId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FactStatus> GetByTypeEntityIdFactAsync(FactEntityType entityType, string entityId, Fact fact, CancellationToken cancellationToken = default)
        {
            var factStatus = await _dbContext.Set<FactStatus>().SingleOrDefaultAsync(
                x => x.EntityType == entityType && x.EntityId == entityId && x.FactId == fact.Id, cancellationToken);
            if (factStatus == null)
            {
                factStatus = new FactStatus()
                {
                    Id = Guid.NewGuid().ToString(),
                    EntityType = entityType,
                    EntityId = entityId,
                    Fact = fact,
                    FactId = fact.Id,
                    Name = fact.Name,
                    Status = false
                };
                await _dbContext.AddAsync(factStatus);
            }
            return factStatus;
        }

        /// <summary>
        /// Get all FactStatus records
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<FactStatus>> GetAllFactStatusAsync(CancellationToken cancellationToken = default)
        {
            var factStatuses = await _dbContext.Set<FactStatus>()
                .Include(x => x.Fact)
                .ToListAsync();
            return factStatuses;
        }
    }
}
