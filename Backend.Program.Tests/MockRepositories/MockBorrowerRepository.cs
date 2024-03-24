using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Domain;
using Backend.Repositories;

namespace Backend.Program.Tests.MockRepositories
{
    internal class MockBorrowerRepository : IBorrowerRepository
    {
        public  ConcurrentBag<Borrower> BorrowerRepository { get; } = new ConcurrentBag<Borrower>();

        public Task<Borrower> GetByBorrowerIdentifierAsync(string borrowerIdentifier, CancellationToken cancellationToken = default)
        {
            var borrower = BorrowerRepository.SingleOrDefault(x => x.Id == borrowerIdentifier);
            if (borrower == null)
            {
                borrower = Borrower.CreateBorrower(borrowerIdentifier);
                BorrowerRepository.Add(borrower);
            }
            return Task.FromResult(borrower);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
