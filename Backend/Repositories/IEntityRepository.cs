using Backend.Domain;

namespace Backend.Repositories
{
    public interface IEntityRepository<T> where T : Entity
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
