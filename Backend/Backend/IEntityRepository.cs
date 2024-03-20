using Backend.Models;

namespace Backend
{
    public interface IEntityRepository<T> where T : Entity
    {
        Task<T?> GeyByIdAsync(Guid? id);
    }
}
