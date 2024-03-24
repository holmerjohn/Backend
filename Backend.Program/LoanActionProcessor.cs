using Backend.Domain;
using Backend.Domain.Loans;
using Backend.Enums;
using Backend.Extensions;
using Backend.Repositories;
using Microsoft.Extensions.Logging;

namespace Backend.Program
{
    public class LoanActionProcessor : ILoanActionProcessor
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IFactEngine _factEngine;
        private readonly ILogger<LoanActionProcessor> _logger;
        private readonly BackendConfiguration _configuration;

        public LoanActionProcessor(
            ILoanRepository loanRepository,
            IBorrowerRepository borrowerRepository,
            IFactEngine factEngine,
            ILogger<LoanActionProcessor> logger,
            BackendConfiguration configuration)
        {
            _loanRepository = loanRepository;
            _borrowerRepository = borrowerRepository;
            _factEngine = factEngine;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ProcessEntityActionsAsync(IEnumerable<EntityAction> actions, CancellationToken cancellationToken = default)
        {
            foreach (var action in actions)
            {
                (IEnumerable<Loan>?, IEnumerable<Borrower>?) results = await ApplyActionAsync(action, cancellationToken);
                foreach (var loan in results.Item1 ?? Enumerable.Empty<Loan>())
                {
                    await _factEngine.ProcessLoanFactsAsync(loan, cancellationToken);
                }
                foreach (var borrower in results.Item2 ?? Enumerable.Empty<Borrower>())
                {
                    await _factEngine.ProcessBorrowerFactsAsync(borrower, cancellationToken);
                }
                await _loanRepository.SaveChangesAsync();
            }
        }

        #region Private Methods

        private async Task<(IEnumerable<Loan>?, IEnumerable<Borrower>?)> ApplyActionAsync(EntityAction action, CancellationToken cancellationToken)
        {
            switch (action.Action)
            {
                case EntityActionType.CreateLoan:
                    {
                        var loan = await _loanRepository.GetByLoanIdentifierAsync(action.LoanIdentifier, cancellationToken);
                        return (null, null);
                    }
                case EntityActionType.CreateBorrower:
                    {
                        var loan = await _loanRepository.GetByLoanIdentifierAsync(action.LoanIdentifier, cancellationToken);
                        if (loan.Borrowers.Any(x => x.Id == action.BorrowerIdentifier))
                        {
                            throw new ApplicationException($"A Borrower with identier '{action.BorrowerIdentifier}' already exists for this loan.");
                        }
                        var borrower = await _borrowerRepository.GetByBorrowerIdentifierAsync(action.BorrowerIdentifier, cancellationToken);
                        loan.Borrowers.Add(borrower);
                        borrower.Loans.Add(loan);
                        return (loan.AsEnumerable(), borrower.AsEnumerable());
                    }
                case EntityActionType.SetLoanField:
                    {
                        var loan = await _loanRepository.GetByLoanIdentifierAsync(action.LoanIdentifier, cancellationToken);
                        loan.SetProperty(action.Field, action.Value);
                        return (loan.AsEnumerable(), loan.Borrowers.AsEnumerable());
                    }
                case EntityActionType.SetBorrowerField:
                    {
                        var borrower = await _borrowerRepository.GetByBorrowerIdentifierAsync(action.BorrowerIdentifier, cancellationToken);
                        borrower.SetProperty(action.Field, action.Value);
                        return (borrower.Loans.AsEnumerable(), borrower.AsEnumerable());
                    }
                default:
                    {
                        throw new ApplicationException("An unsupported ActionType was received.");
                    }
            }
        }

        #endregion
    }
}
