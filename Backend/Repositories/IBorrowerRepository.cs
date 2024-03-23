using Backend.Domain;

namespace Backend.Repositories
{
    public interface IBorrowerRepository : IEntityRepository<Borrower>
    {
        Task<Borrower> GetByBorrowerIdentifierAsync(string loanIdentifier, CancellationToken cancellationToken = default);
    }
}
