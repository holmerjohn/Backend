using Backend.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Domain
{
    public class EntityAction
    {
        private object _value = null;

        [JsonPropertyName("action")]
        public EntityActionType Action { get; init; }
        [JsonPropertyName("loanIdentifier")]
        public string? LoanIdentifier { get; init; }
        [JsonPropertyName("borrowerIdentifier")]
        public string? BorrowerIdentifier { get; init; }
        [JsonIgnore]
        public PropertyType PropertyType { get; private set; }
        [JsonPropertyName("field")]
        public string? Field { get; init; }
        [JsonPropertyName("value")]
        public object? Value
        {
            get { return _value; }
            init
            {
                if (value?.GetType() == typeof(JsonElement))
                {
                    var jsonElement = (JsonElement)value;
                    SetValueFieldsFronJson(jsonElement);
                }
                else
                {
                    SetValueFieldsFromObject(value);
                }
            }
        }
        private void SetValueFieldsFronJson(JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Null:
                    PropertyType = PropertyType.Null;
                    _value = null;
                    break;
                case JsonValueKind.String:
                    PropertyType = PropertyType.String;
                    _value = jsonElement.GetString();
                    break;
                case JsonValueKind.Number:
                    PropertyType = PropertyType.Number;
                    _value = jsonElement.GetDecimal();
                    break;
            }
        }
        private void SetValueFieldsFromObject(object? value)
        {
            if (value == null)
            {
                _value = null;
                PropertyType = PropertyType.Null;
                return;
            }
            if (value.GetType() == typeof(string))
            {
                PropertyType = PropertyType.String;
                _value = value.ToString();
            }
            else
            {
                PropertyType = PropertyType.Number;
                _value = Convert.ToDecimal(value);
            }
        }
    }
}
