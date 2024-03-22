using Backend.Domain;
using System.Text.Json;

namespace Backend.Program
{
    public class ActionStore : IActionStore
    {
        public async Task<IEnumerable<EntityAction>> GetActionsAsync(Stream utf8json, CancellationToken cancellationToken = default)
        {
            var actions = await JsonSerializer.DeserializeAsync<IEnumerable<EntityAction>>(utf8json, cancellationToken: cancellationToken);
            return actions;
        }
    }
}
