﻿namespace Backend.Domain
{
    public class Borrower : Entity
    {
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public virtual ICollection<BorrowerProperty> BorrowerProperties { get; set; } = new List<BorrowerProperty>();

        public static Borrower CreateBorrower(string identifier)
        {
            return new Borrower()
            {
                Id = identifier
            };
        }
        public void SetProperty(string propertyName, object? value)
        {
            var property = BorrowerProperties.SingleOrDefault(x => x.Name == propertyName);
            if (null == property)
            {
                property = new BorrowerProperty()
                {
                    Id = $"{Id}-{propertyName}",
                    BorrowerId = Id,
                    Name = propertyName
                };

                if (value != null)
                {
                    property.ValueAsString = value.ToString();
                }
                BorrowerProperties.Add(property);
            }
            else
            {
                property.ValueAsString = value?.ToString();
            }
        }
    }
}
