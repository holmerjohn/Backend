using Backend.Domain;

namespace Backend
{
    public interface ILoanActionProcessor
    {
        Task ProcessEntityActionsAsync(IEnumerable<EntityAction> actions, CancellationToken cancellationToken = default);
    }
}
