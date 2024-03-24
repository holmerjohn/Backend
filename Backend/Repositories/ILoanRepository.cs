using Backend.Domain.Loans;

namespace Backend.Repositories
{
    public interface ILoanRepository : IEntityRepository<Loan>
    {
        Task<Loan> GetByLoanIdentifierAsync(string loanIdentifier, CancellationToken cancellationToken = default);
    }
}
