using Backend.Domain;
using Backend.Enums;
using Backend.Extensions;
using System.Text.Json;

namespace Backend.Program
{
    public class ActionEngine : IActionEngine
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private IEnumerable<EntityAction>? _actions;

        public ActionEngine(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }
        public IEnumerable<EntityAction> Actions => _actions?.AsEnumerable() ?? throw new ApplicationException($"ActionStore not yet loaded.  Please call {nameof(LoadActionsAsync)} first.");

        public async Task LoadActionsAsync(Stream utf8json, CancellationToken cancellationToken = default)
        {
            var inputActions = await JsonSerializer.DeserializeAsync<IEnumerable<Models.EntityAction>>(utf8json, _jsonSerializerOptions, cancellationToken: cancellationToken);
            _actions = inputActions.Select(inputAction =>
            {
                PropertyType pt = PropertyType.Null;
                Object? value = null;
                if (inputAction.Value?.GetType() == typeof(JsonElement))
                {
                    var jsonElement = (JsonElement)inputAction.Value;
                    pt = jsonElement.GetPropertyType();
                    value = GetValueFromJson(jsonElement);
                }
                else
                {
                    pt = inputAction.Value.GetPropertyType();
                    value = GetValueFromObject(inputAction.Value);
                }

                return new EntityAction()
                {
                    Action = inputAction.Action,
                    LoanIdentifier = inputAction.LoanIdentifier,
                    BorrowerIdentifier = inputAction.BorrowerIdentifier,
                    Field = inputAction.Field,
                    PropertyType = pt,
                    Value = value
                };
            }).AsEnumerable();
        }

        private object? GetValueFromJson(JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Null:
                    return null;
                case JsonValueKind.String:
                    return jsonElement.GetString();
                case JsonValueKind.Number:
                    return jsonElement.GetDecimal();
                default:
                    return null;
            }
        }

        private object? GetValueFromObject(object? value)
        {
            switch (value)
            {
                case null:
                    return null;
                case string _:
                    return value.ToString();
                default:
                    return Convert.ToDecimal(value);
            }
        }
    }
}
