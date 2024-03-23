using Backend.Enums;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class EntityAction
    {
        [JsonPropertyName("action")]
        public EntityActionType Action { get; init; }
        [JsonPropertyName("loanIdentifier")]
        public string? LoanIdentifier { get; init; }
        [JsonPropertyName("borrowerIdentifier")]
        public string? BorrowerIdentifier { get; init; }
        [JsonPropertyName("field")]
        public string? Field { get; init; }
        [JsonPropertyName("value")]
        public object? Value { get; init; }
    }
}
