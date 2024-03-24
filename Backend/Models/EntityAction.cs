using Backend.Enums;
using Backend.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class EntityAction
    {
        private object? _value = null;

        [JsonPropertyName("action")]
        public EntityActionType Action { get; init; }
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
                /* 
                 * When deserializing, it will save the actual JsonElement object
                 * to the object's value field.  We want the actual value.
                 * */
                if (value?.GetType() == typeof(JsonElement))
                {
                    var jsonElement = (JsonElement)value;
                    _value = jsonElement.GetValueFromJson();
                }
                else
                {
                    _value = value;
                }
            }
        }
    }
}
