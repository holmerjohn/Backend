using Backend.Domain.Loans;
using Backend.Repositories;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Program.Tests.MockRepositories
{
    internal class MockLoanRepository : ILoanRepository
    {
        public ConcurrentBag<Loan> LoanRepository { get; } = new ConcurrentBag<Loan>();

        public Task<Loan> GetByLoanIdentifierAsync(string loanIdentifier, CancellationToken cancellationToken = default)
        {
            var loan = LoanRepository.SingleOrDefault(x => x.Id == loanIdentifier);
            if (loan == null)
            {
                loan = Loan.CreateLoan(loanIdentifier);
                LoanRepository.Add(loan);
            }
            return Task.FromResult(loan);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        { 
            return Task.CompletedTask;
        }
    }
}
