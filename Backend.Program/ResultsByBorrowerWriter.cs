using Backend.Repositories;

namespace Backend.Program
{
    internal class ResultsByBorrowerWriter : IResultsWriter
    {
        private readonly IFactStatusRepository _factStatusRepository;
        private readonly IBorrowerRepository _borrowerRepository;

        public ResultsByBorrowerWriter(
            IFactStatusRepository factStatusRepository,
            IBorrowerRepository borrowerRepository)
        {
            _factStatusRepository = factStatusRepository;
            _borrowerRepository = borrowerRepository;
        }

        public async Task WriteResults(CancellationToken cancellationToken = default)
        {
            Console.WriteLine();
            Console.WriteLine();
            var factStatuses = await _factStatusRepository.GetAllFactStatusAsync();

            foreach (var borrowerGrouping in factStatuses
                .Where(x => x.EntityType == Enums.FactEntityType.Borrower)
                .GroupBy(x => x.EntityId, x => x, (entityId, statuses) => new { BorrowerId = entityId, Statuses = statuses }))
            {
                var borrower = await _borrowerRepository.GetByBorrowerIdentifierAsync(borrowerGrouping.BorrowerId);
                var loanIds = borrower.Loans.Select(x => x.Id);

                Console.WriteLine($"{"********* Borrower".PadLeft(20)} ---> {borrowerGrouping.BorrowerId}");
                foreach (var status in borrowerGrouping.Statuses)
                {
                    Console.WriteLine($"{"Fact".PadLeft(20)} ---> {status.Name.PadRight(40)}   Status: {status.Status}");
                }

                foreach (var loanId in loanIds)
                {
                    var loanGroupings = factStatuses
                        .Where(x => x.EntityType == Enums.FactEntityType.Loan && x.EntityId == loanId)
                        .GroupBy(x => x.EntityId, x => x, (entityId, statuses) => new { LoanId = entityId, Statuses = statuses });
                    foreach (var loanGrouping in loanGroupings)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"{"Loan".PadLeft(20)} ---> {loanGrouping.LoanId.PadRight(40)}");
                        foreach (var status in loanGrouping.Statuses)
                        {
                            Console.WriteLine($"{"Fact".PadLeft(20)} ---> {status.Name.PadRight(40)}   Status: {status.Status}");
                        }
                    }

                }
                Console.WriteLine();
            }
        }
    }
}
