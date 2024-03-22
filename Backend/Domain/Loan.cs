using System.Text.Json.Serialization;

namespace Backend.Domain
{
    public class Loan : Entity
    {
        public virtual ICollection<LoanProperty> LoanProperties { get; set; } = new List<LoanProperty>();

        public ICollection<Borrower> Borrowers { get; set; } = new List<Borrower>();

        public static Loan CreateLoan(string identifier)
        {
            return new Loan() 
            {
                Id =  identifier
            };
        }

        public void SetProperty(string propertyName, object? value)
        {
            var property = LoanProperties.SingleOrDefault(x => x.Name == propertyName);
            if (null == property)
            {
                property = new LoanProperty()
                {
                    Id = $"{Id}-{propertyName}",
                    LoanId = Id,
                    Name = propertyName
                };

                if (value != null)
                {
                    property.ValueAsString = value.ToString();
                }
                LoanProperties.Add(property);
            }
            else
            {
                property.ValueAsString = value?.ToString();
            }
        }
    }
}
