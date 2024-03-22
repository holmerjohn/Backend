using Backend.Domain;

namespace Backend
{
    public interface IEntityActionProcessor
    {
        Task ProcessEntityActionsAsync(IEnumerable<EntityAction> actions, CancellationToken cancellationToken = default);
    }
}
