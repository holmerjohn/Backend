namespace Backend.Domain
{
    public class BorrowerProperty : BaseProperty
    {
        public string BorrowerId { get; set; }
        public virtual Borrower Borrower { get; set; }
    }
}
