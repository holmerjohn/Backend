using Backend.Domain;

namespace Backend
{
    public interface IActionEngine
    {
        Task LoadActionsAsync(Stream utf8json, CancellationToken cancellationToken = default);

        public IEnumerable<EntityAction> Actions { get;}
    }
}
