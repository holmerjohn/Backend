using Backend.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories
{
    internal class LoanRepository : EntityRepository<Loan>, ILoanRepository
    {
        public LoanRepository(BackendDbContext dbContext)
            : base(dbContext)
        { }

        /// <summary>
        /// Get a Loan by its identifier.
        /// </summary>
        /// <remarks>This will create a new loan object if one is not found for that id.  As such
        /// it should never return a null value.</remarks>
        /// <param name="loanIdentifier"></param>
        /// <returns></returns>
        public async Task<Loan> GetByLoanIdentifierAsync(string loanIdentifier, CancellationToken cancellationToken = default)
        {
            var loan = await _dbContext.Set<Loan>().SingleOrDefaultAsync(x => x.Id == loanIdentifier);
            if (loan == null)
            {
                loan = Loan.CreateLoan(loanIdentifier);
                await _dbContext.Set<Loan>().AddAsync(loan);
            }
            return loan;
        }
    }
}
