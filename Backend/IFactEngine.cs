using Backend.Domain.Loans;
using Backend.Domain.Facts;

namespace Backend
{
    public interface IFactEngine
    {
        Task LoadFactsAsync(Stream utf8json, CancellationToken cancellationToken = default);
        Task ProcessLoanFactsAsync(Loan loan, CancellationToken cancellationToken = default);
        Task ProcessBorrowerFactsAsync(Borrower borrower, CancellationToken cancellationToken = default);
    }
}
