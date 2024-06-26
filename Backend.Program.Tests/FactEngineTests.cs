﻿using Backend.Converters;
using Backend.Domain.Facts;
using Backend.Domain.Loans;
using Backend.Enums;
using Backend.Extensions;
using Backend.Program.Tests.MockRepositories;
using Backend.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Program.Tests
{
    public class FactEngineTests
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        public FactEngineTests()
        {
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Converters =
                        {
                            new ConditionComparatorJsonConverter(),
                            new EntityActionTypeJsonConverter(),
                            new FactEntityTypeJsonConverter()
                        }
            };
        }


        [Fact]
        public async Task ShouldGetFacts()
        {

            var fact = new Fact()
            {
                Id = "f123",
                Name = "Loan is purchase loan",
                EntityType = FactEntityType.Loan
            };
            fact.Conditions.Add(new FactCondition()
            {
                Id = "fc123",
                FactId = fact.Id,
                Field = "loanType",
                Comparator = ConditionComparator.Equals,
                PropertyType = PropertyType.String,
                Value = "Purchase"
            });

            var factRepositoryMock = new Mock<IFactRepository>();
            factRepositoryMock
                .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(fact.AsEnumerable()));
            var mockFactStatusRepository = new MockFactStatusRepository();
            var loggerMock = new Mock<ILogger<FactEngine>>();

            var factEngine = new FactEngine(
                factRepositoryMock.Object,
                mockFactStatusRepository,
                loggerMock.Object,
                _jsonSerializerOptions);

            var loan = new Loan()
            {
                Id = "abc",
                LoanProperties = new List<LoanProperty>()
                {
                    new LoanProperty() { Id = "l123", LoanId = "abc", Name = "loanAmount", PropertyType = PropertyType.Number, NumberValue = 100000},
                    new LoanProperty() { Id = "l234", LoanId = "abc", Name = "loanType", PropertyType = PropertyType.String, StringValue = "Purchase"},
                    new LoanProperty() { Id = "l345", LoanId = "abc", Name = "purchasePrice", PropertyType = PropertyType.Number, NumberValue = 500000}
                }
            };

            await factEngine.ProcessLoanFactsAsync(loan);

        }
    }
}
