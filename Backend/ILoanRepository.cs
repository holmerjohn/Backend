using Backend.Domain;

namespace Backend
{
    public interface ILoanRepository : IEntityRepository<Loan>
    {
        Task<Loan> GetByLoanIdentifierAsync(string loanIdentifier, CancellationToken cancellationToken = default);
    }
}
