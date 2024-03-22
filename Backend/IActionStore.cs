using Backend.Domain;

namespace Backend
{
    public interface IActionStore
    {
        Task<IEnumerable<EntityAction>> GetActionsAsync(Stream utf8json, CancellationToken cancellationToken = default);
    }
}
