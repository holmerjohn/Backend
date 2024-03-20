using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Loan : Entity
    {
        [JsonPropertyName("loanAmount")]
        public decimal? LoanAmount { get; set; }
        [JsonPropertyName("loanType")]
        public string? LoanType { get; set; }
        [JsonPropertyName("purchasePrice")]
        public decimal? PurchasePrice { get; set; }
        [JsonPropertyName("propertyAddress")]
        public string? PropertyAddress { get; set; }
        [JsonPropertyName("borrowers")]
        public ICollection<Borrower> Borrowers { get; set; } = new List<Borrower>();
    }
}
