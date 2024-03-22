using Backend.Domain;

namespace Backend
{
    public interface IEntityRepository<T> where T : Entity
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
