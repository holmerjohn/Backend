using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Domain
{
    public class EntityAction
    {
        private object _value = null;
        [JsonPropertyName("action")]
        public string ActionAsString { get; init; }
        [JsonPropertyName("loanIdentifier")]
        public string? LoanIdentifier { get; init; }
        [JsonPropertyName("borrowerIdentifier")]
        public string? BorrowerIdentifier { get; init; }
        [JsonPropertyName("field")]
        public string? Field { get; init; }
        [JsonPropertyName("value")]
        public object? Value
        {
            get { return _value; }
            init
            {
                if (value == null)
                {
                    _value = null;
                    return;
                }
                if (value.GetType() == typeof(JsonElement))
                {
                    var jsonElement = (JsonElement)value;
                    switch (jsonElement.ValueKind)
                    {
                        case JsonValueKind.Null:
                            _value = null;
                            break;
                        case JsonValueKind.String:
                            _value = jsonElement.GetString();
                            break;
                        case JsonValueKind.Number:
                            _value = jsonElement.GetDecimal();
                            break;
                    }
                }
                else
                {
                    _value = value;
                }
            }
        }

        [JsonIgnore]
        public EntityActionType ActionType
        {
            get
            {
                if (Enum.TryParse<EntityActionType>(ActionAsString, true, out EntityActionType actionType))
                {
                    return actionType;
                }
                else
                {
                    return EntityActionType.Unsupported;
                }
            }
        }
    }
}
