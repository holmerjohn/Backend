namespace Backend.Domain
{
    public class LoanProperty : Entity
    {
        public string LoanId { get; set; }
     
        public virtual Loan Loan { get; set; }

        public string Name { get; set; }
        public string? ValueAsString { get; set; }
    }
}
