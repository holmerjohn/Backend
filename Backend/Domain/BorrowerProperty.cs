namespace Backend.Domain
{
    public class BorrowerProperty : Entity
    {
        public string BorrowerId { get; set; }
        public virtual Borrower Borrower { get; set; }
        
        public string Name { get; set; }
        public string? ValueAsString { get; set; }
    }
}
