using Backend.Domain.Facts;

namespace Backend.Repositories
{
    public interface IFactRepository : IEntityRepository<Fact>
    {
        Fact? GetByName(string? name);
        Task<Fact?> GetByNameAsync(string? name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Fact>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
