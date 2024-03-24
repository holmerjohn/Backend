using Backend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Program
{
    internal class ResultsByLoanWriter : IResultsWriter
    {
        private readonly IFactStatusRepository _factStatusRepository;
        private readonly ILoanRepository _loanRepository;

        public ResultsByLoanWriter(
            IFactStatusRepository factStatusRepository,
            ILoanRepository loanRepository)
        {
            _factStatusRepository = factStatusRepository;
            _loanRepository = loanRepository;
        }

        public async Task WriteResults(CancellationToken cancellationToken = default)
        {
            Console.WriteLine();
            Console.WriteLine();

            var factStatuses = await _factStatusRepository.GetAllFactStatusAsync();

            foreach (var loanGrouping in factStatuses
                .Where(x => x.EntityType == Enums.FactEntityType.Loan)
                .GroupBy(x => x.EntityId, x => x, (entityId, statuses) => new { LoanId = entityId, Statuses = statuses }))
            {
                var loan = await _loanRepository.GetByLoanIdentifierAsync(loanGrouping.LoanId);
                var borrowerIds = loan.Borrowers.Select(x => x.Id);

                Console.WriteLine($"{"********* Loan".PadLeft(20)} ---> {loanGrouping.LoanId}");
                foreach (var status in loanGrouping.Statuses)
                {
                    Console.WriteLine($"{"Fact".PadLeft(20)} ---> {status.Name.PadRight(40)}   Status: {status.Status}");
                }

                foreach (var borrowerId in borrowerIds)
                {
                    var borrowerGroupings = factStatuses
                        .Where(x => x.EntityType == Enums.FactEntityType.Borrower && x.EntityId == borrowerId)
                        .GroupBy(x => x.EntityId, x => x, (entityId, statuses) => new { BorrowerId = entityId, Statuses = statuses });
                    foreach (var borrowerGrouping in borrowerGroupings)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"{"Borrower".PadLeft(20)} ---> {borrowerGrouping.BorrowerId.PadRight(40)}");
                        foreach (var status in borrowerGrouping.Statuses)
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
