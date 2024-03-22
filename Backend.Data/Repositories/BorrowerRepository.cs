using Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories
{
    internal class BorrowerRepository : EntityRepository<Borrower>, IBorrowerRepository
    {
        public BorrowerRepository(BackendDbContext dbContext)
            : base(dbContext)
        { }

        /// <summary>
        /// Get a Borrower by its identifier.
        /// </summary>
        /// <remarks>This will create a new borrower object if one is not found for that id.  As such
        /// it should never return a null value.</remarks>
        /// <param name="borrowerIdentifier"></param>
        /// <returns></returns>
        public async Task<Borrower> GetByBorrowerIdentifierAsync(string borrowerIdentifier, CancellationToken cancellationToken = default)
        {
            var borrower = await _dbContext.Set<Borrower>().SingleOrDefaultAsync(x => x.Id == borrowerIdentifier);
            if (borrower == null)
            {
                borrower = Borrower.CreateBorrower(borrowerIdentifier);
                await _dbContext.Set<Borrower>().AddAsync(borrower);
            }
            return borrower;
        }
    }
}
