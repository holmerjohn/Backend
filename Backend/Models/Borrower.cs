using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Borrower : Entity
    {
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("address")]
        public string? Address { get; set; }
        [JsonPropertyName("birthYear")]
        public int? BirthYear { get; set; }

        [JsonIgnore]
        public Guid LoanId { get; set; }
        [JsonIgnore]
        public virtual Loan Loan { get; set; }
    }
}
