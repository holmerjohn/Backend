using Backend.Enums;
using System.Dynamic;

namespace Backend.Domain.Loans
{
    public class Loan : Entity
    {
        public virtual ICollection<LoanProperty> LoanProperties { get; set; } = new List<LoanProperty>();

        public ICollection<Borrower> Borrowers { get; set; } = new List<Borrower>();

        public static Loan CreateLoan(string identifier)
        {
            return new Loan()
            {
                Id = identifier
            };
        }

        public void SetProperty(string propertyName, object? value)
        {
            var property = LoanProperties.SingleOrDefault(x => x.Name == propertyName);
            if (null == property)
            {
                property = new LoanProperty()
                {
                    Id = Guid.NewGuid().ToString(),
                    LoanId = Id,
                    Name = propertyName
                };
                property.SetPropertyValue(value);
                LoanProperties.Add(property);
            }
            else
            {
                property.SetPropertyValue(value);
            }
        }

        public dynamic ToDynamic()
        {
            var loan = new ExpandoObject() as IDictionary<string, object>;
            var borrowers = Borrowers.Select(borrower => 
            { 
                var b = new ExpandoObject() as IDictionary<string,Object>;
                foreach (var bp in borrower.BorrowerProperties)
                {
                    switch (bp.PropertyType)
                    {
                        case PropertyType.String:
                            b.Add(bp.Name, bp.StringValue);
                            break;
                        case PropertyType.Number:
                            b.Add(bp.Name, bp.NumberValue);
                            break;
                        case PropertyType.Null:
                            b.Add(bp.Name, null);
                            break;
                    }
                }
                return b;
            }).ToArray();

            foreach (var lp in LoanProperties)
            {
                switch (lp.PropertyType)
                {
                    case PropertyType.String:
                        loan.Add(lp.Name, lp.StringValue);
                        break;
                    case PropertyType.Number:
                        loan.Add(lp.Name, lp.NumberValue);
                        break;
                    case PropertyType.Null:
                        loan.Add(lp.Name, null);
                        break;
                }
            }
            loan.Add("borrowers", borrowers);
            return loan;
        }
    }
}
