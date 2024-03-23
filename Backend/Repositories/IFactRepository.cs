using Backend.Domain.Facts;

namespace Backend.Repositories
{
    public interface IFactRepository : IEntityRepository<Fact>
    {
        Task<Fact> GetByNameAsync(string? factIdentifier, CancellationToken cancellationToken = default);
    }
}
