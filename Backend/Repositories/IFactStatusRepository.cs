using Backend.Domain.Facts;
using Backend.Enums;

namespace Backend.Repositories
{
    public interface IFactStatusRepository : IEntityRepository<FactStatus>
    {
        Task<IEnumerable<FactStatus>> GetAllFactStatusAsync(CancellationToken cancellationToken = default);
        Task<FactStatus> GetByTypeEntityIdFactAsync(FactEntityType entityType, string entityId, Fact fact, CancellationToken cancellationToken = default);
    }
}
