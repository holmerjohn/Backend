using Backend.Enums;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Fact
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("entityType")]
        public FactEntityType EntityType { get; set; }

        [JsonPropertyName("conditions")]
        public virtual ICollection<FactCondition> Conditions { get; set; } = new List<FactCondition>();

    }
}
