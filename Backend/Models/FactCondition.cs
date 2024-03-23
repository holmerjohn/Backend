using Backend.Domain.Facts;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class FactCondition
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }
        [JsonPropertyName("comparator")]
        public ConditionComparator Comparator { get; set; }
        [JsonPropertyName("value")]
        public object? Value { get; set; }
    }
}
