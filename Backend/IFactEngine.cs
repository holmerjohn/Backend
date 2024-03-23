using Backend.Domain;

namespace Backend
{
    public interface IFactEngine
    {
        Task LoadFactsAsync(Stream utf8json, CancellationToken cancellationToken = default);
        Task ProcessLoanFactsAsync(Loan loan, CancellationToken cancellationToken = default);
    }
}
