﻿using Backend.Domain;
using Backend.Enums;
using Microsoft.Extensions.Logging;

namespace Backend.Program
{
    public class EntityActionProcessor : IEntityActionProcessor
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly ILogger<EntityActionProcessor> _logger;
        private readonly BackendConfiguration _configuration;

        public EntityActionProcessor(
            ILoanRepository loanRepository,
            IBorrowerRepository borrowerRepository,
            ILogger<EntityActionProcessor> logger,
            BackendConfiguration configuration)
        {
            _loanRepository = loanRepository;
            _borrowerRepository = borrowerRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ProcessEntityActionsAsync(IEnumerable<EntityAction> actions, CancellationToken cancellationToken = default)
        {
            foreach (var action in actions)
            {
                IEnumerable<string> loanIdentifiersToProcess = await ApplyActionAsync(action, cancellationToken);
                await _loanRepository.SaveChangesAsync();
            }
            

        }
        private async Task<IEnumerable<string>> ApplyActionAsync(EntityAction action, CancellationToken cancellationToken)
        {
            switch (action.Action)
            {
                case EntityActionType.CreateLoan:
                    {
                        await _loanRepository.GetByLoanIdentifierAsync(action.LoanIdentifier, cancellationToken);
                        return new List<string>() { action.LoanIdentifier };
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
                        return new List<string>() { action.LoanIdentifier };
                    }
                case EntityActionType.SetLoanField:
                    {
                        var loan = await _loanRepository.GetByLoanIdentifierAsync(action.LoanIdentifier, cancellationToken);
                        loan.SetProperty(action.Field, action.Value);
                        return new List<string>() { action.LoanIdentifier };
                    }
                case EntityActionType.SetBorrowerField:
                    {
                        var borrower = await _borrowerRepository.GetByBorrowerIdentifierAsync(action.BorrowerIdentifier, cancellationToken);
                        borrower.SetProperty(action.Field, action.Value);
                        return borrower.Loans.Select(x => x.Id);
                    }
                default:
                    {
                        throw new ApplicationException("An unsupported ActionType was received.");
                    }
            }
        }
    }
}
