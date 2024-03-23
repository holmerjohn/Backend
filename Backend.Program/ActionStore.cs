using Backend.Domain;
using Backend.Converters;
using System.Text.Json;

namespace Backend.Program
{
    public class ActionStore : IActionStore
    {
        public async Task<IEnumerable<EntityAction>> GetActionsAsync(Stream utf8json, CancellationToken cancellationToken = default)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new EntityActionTypeJsonConverter()
                }
            };
            var actions = await JsonSerializer.DeserializeAsync<IEnumerable<EntityAction>>(utf8json, options, cancellationToken: cancellationToken);
            return actions;
        }
    }
}
