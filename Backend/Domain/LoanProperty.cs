namespace Backend.Domain
{
    public class LoanProperty : BaseProperty
    {
        public string LoanId { get; set; }
     
        public virtual Loan Loan { get; set; }
    }
}
