using Backend.Domain.Loans;
using Backend.Domain.Facts;
using Backend.Enums;
using Backend.Extensions;
using Backend.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Backend.Program
{
    public class FactEngine : IFactEngine
    {

        private readonly IFactRepository _factRepository;
        private readonly IFactStatusRepository _factStatusRepository;
        private readonly ILogger<FactEngine> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public FactEngine(
            IFactRepository factRepository,
            IFactStatusRepository factStatusRepository,
            ILogger<FactEngine> logger,
            JsonSerializerOptions jsonSerializerOptions)
        {
            _factRepository = factRepository;
            _factStatusRepository = factStatusRepository;
            _logger = logger;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task LoadFactsAsync(Stream utf8json, CancellationToken cancellationToken = default)
        {
            var inputFacts = await JsonSerializer.DeserializeAsync<IEnumerable<Models.Fact>>(utf8json,
                _jsonSerializerOptions, cancellationToken: cancellationToken);
            foreach (var inputFact in inputFacts)
            {
                var dbFact = await _factRepository.GetByNameAsync(inputFact.Name);
                dbFact.EntityType = inputFact.EntityType;
                foreach (var inputCondition in inputFact.Conditions)
                {
                    PropertyType pt = PropertyType.Null;
                    Object? value = null;
                    if (inputCondition.Value?.GetType() == typeof(JsonElement))
                    {
                        var jsonElement = (JsonElement)inputCondition.Value;
                        pt = jsonElement.GetPropertyType();
                        value = jsonElement.GetValueFromJson();
                    }
                    else
                    {
                        pt = inputCondition.Value.GetPropertyType();
                        value = inputCondition.Value.GetValueFromObject();
                    }
                    var dbCondition = new FactCondition()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FactId = dbFact.Id,
                        Field = inputCondition.Field,
                        Comparator = inputCondition.Comparator,
                        PropertyType = pt,
                        Value = value?.ToString()
                    };
                    dbFact.Conditions.Add(dbCondition);
                }
            }
            await _factRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task ProcessLoanFactsAsync(Loan loan, CancellationToken cancellationToken = default)
        {
            IDictionary<string, bool> factResults = new Dictionary<string, bool>();
            var facts = await _factRepository.GetAllAsync(cancellationToken);

            foreach (var fact in facts.Where(fact => fact.EntityType == FactEntityType.Loan))
            {
                var satisfiesFact = ProcessLoanFact(ref factResults, fact, loan);

                // Save the result to the database
                var factStatus = await _factStatusRepository.GetByTypeEntityIdFactAsync(FactEntityType.Loan, loan.Id, fact, cancellationToken);
                factStatus.Status = satisfiesFact;
            }
        }

        private bool ProcessLoanFact(ref IDictionary<string, bool> factResults, Fact fact, Loan loan)
        {
            var key = $"Loan-{fact.Name}";

            _logger.LogDebug("Starting to evaluate loan '{loanId}' for fact '{factName}'", loan.Id, fact.Name);

            if (factResults.ContainsKey(key))
                return factResults[key];

            bool satisfiesConditions = EvaluateLoanConditions(ref factResults, fact, loan);

            Console.WriteLine($"{"Loan Fact ->".PadLeft(18)} {fact.Name.PadRight(40)} {loan.Id} : {satisfiesConditions}");

            factResults[key] = satisfiesConditions;
            return satisfiesConditions;
        }

        private bool EvaluateLoanConditions(ref IDictionary<string, bool> factResults, Fact fact, Loan loan)
        {
            // A fact without conditions is, by definition, false
            if (!fact.Conditions.Any())
                return false;

            bool satisfiesConditions = true;
            foreach (var condition in fact.Conditions)
            {
                // If it has already been determined, we should not have made it to this method
                var property = loan.LoanProperties.SingleOrDefault(x => x.Name == condition.Field);
                if (satisfiesConditions && property != null)
                {
                    satisfiesConditions &= condition.IsSatisfiedByProperty(property);
                    continue;
                }

                // Is it a fact that hasn't been determined
                var conditionFact = _factRepository.GetByName(condition.Field);
                if (satisfiesConditions && conditionFact != null)
                {
                    // Be prepared for the rule to cross domain boundaries
                    if (conditionFact.EntityType == FactEntityType.Loan)
                    {
                        satisfiesConditions &= EvaluateLoanConditions(ref factResults, conditionFact, loan);
                    }
                    else
                    {
                        foreach (var borrower in loan.Borrowers)
                        {
                            satisfiesConditions &= EvaluateBorrowerConditions(ref factResults, conditionFact, borrower);
                        }
                    }
                    continue;
                }

                // No property or fact can resolve this condition, therefore it is false
                satisfiesConditions = false;
            };

            return satisfiesConditions;
        }

        public async Task ProcessBorrowerFactsAsync(Borrower borrower, CancellationToken cancellationToken = default)
        {
            IDictionary<string, bool> factResults = new Dictionary<string, bool>();
            var facts = await _factRepository.GetAllAsync(cancellationToken);

            foreach (var fact in facts.Where(fact => fact.EntityType == FactEntityType.Borrower))
            {
                var satisfiesFact = ProcessBorrowerFact(ref factResults, fact, borrower);

                // Save the result to the database
                var factStatus = await _factStatusRepository.GetByTypeEntityIdFactAsync(FactEntityType.Borrower, borrower.Id, fact, cancellationToken);
                factStatus.Status = satisfiesFact;
            }
        }

        private bool ProcessBorrowerFact(ref IDictionary<string, bool> factResults, Fact fact, Borrower borrower)
        {
            var key = $"Borrower-{borrower.Id}-{fact.Name}";

            _logger.LogDebug("Starting to evaluate borrower {borrowerId} for fact {factName}", borrower.Id, fact.Name);

            if (factResults.ContainsKey(key))
                return factResults[key];

            bool satisfiesConditions = EvaluateBorrowerConditions(ref factResults, fact, borrower);

            Console.WriteLine($"{"Borrower Fact ->".PadLeft(18)} {fact.Name.PadRight(40)} {borrower.Id} : {satisfiesConditions}");

            factResults[key] = satisfiesConditions;
            return satisfiesConditions;
        }

        private bool EvaluateBorrowerConditions(ref IDictionary<string, bool> factResults, Fact fact, Borrower borrower)
        {
            // A fact without conditions is, by definition, false
            if (!fact.Conditions.Any())
                return false;

            bool satisfiesConditions = true;
            foreach (var condition in fact.Conditions)
            {
                // If it has already been determined, we should not have made it to this method
                var property = borrower.BorrowerProperties.SingleOrDefault(x => x.Name == condition.Field);
                if (satisfiesConditions && property != null)
                {
                    satisfiesConditions &= condition.IsSatisfiedByProperty(property);
                    continue;
                }

                // Is it a fact that hasn't been determined
                var conditionFact = _factRepository.GetByName(condition.Field);
                if (satisfiesConditions && conditionFact != null)
                {
                    // Be prepared for the rule to cross domain boundaries
                    if (conditionFact.EntityType == FactEntityType.Borrower)
                    {
                        satisfiesConditions &= EvaluateBorrowerConditions(ref factResults, conditionFact, borrower);
                    }
                    else
                    {
                        foreach (var loan in borrower.Loans)
                        {
                            satisfiesConditions &= EvaluateLoanConditions(ref factResults, conditionFact, loan);
                        }
                    }
                    continue;
                }

                // No property or fact can resolve this condition, therefore it is false
                satisfiesConditions = false;
            };

            return satisfiesConditions;
        }
    }
}
