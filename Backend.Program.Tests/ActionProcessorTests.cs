using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Backend.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Backend.Program.Tests
{
    public class ActionProcessorTests
    {
        [Fact]
        public async Task ShouldProcess_CreateLoan()
        {
            var mockLoanRepository = new MockLoanRepository();
            var loggerMock = new Mock<ILogger<EntityActionProcessor>>();

            IEntityActionProcessor processor = new EntityActionProcessor(
                mockLoanRepository,
                new MockBorrowerRepository(),
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { ActionAsString = "createLoan", LoanIdentifier = "abc" }
            };

            await processor.ProcessEntityActionsAsync(actionList.AsEnumerable());

            mockLoanRepository.LoanRepository.Count.Should().Be(1);
            var loan = mockLoanRepository.LoanRepository.First();

            loan.Id.Should().Be("abc");
        }

        [Fact]
        public async Task ShouldProcess_CreateBorrower()
        {
            var mockLoanRepository = new MockLoanRepository();
            var mockBorrowerRepository = new MockBorrowerRepository();
            var loggerMock = new Mock<ILogger<EntityActionProcessor>>();

            IEntityActionProcessor processor = new EntityActionProcessor(
                mockLoanRepository,
                mockBorrowerRepository,
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { ActionAsString = "createLoan", LoanIdentifier = "abc" },
                new EntityAction() { ActionAsString = "createBorrower", LoanIdentifier = "abc", BorrowerIdentifier = "def" }
            };

            await processor.ProcessEntityActionsAsync(actionList.AsEnumerable());

            mockLoanRepository.LoanRepository.Count.Should().Be(1);
            var loan = mockLoanRepository.LoanRepository.First();
            loan.Id.Should().Be("abc");

            mockBorrowerRepository.BorrowerRepository.Count.Should().Be(1);
            var borrower = mockBorrowerRepository.BorrowerRepository.First();
            borrower.Id.Should().Be("def");
        }

        [Fact]
        public async Task ShouldProcess_SetLoanField()
        {
            var mockLoanRepository = new MockLoanRepository();
            var loggerMock = new Mock<ILogger<EntityActionProcessor>>();

            IEntityActionProcessor processor = new EntityActionProcessor(
                mockLoanRepository,
                new MockBorrowerRepository(),
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { ActionAsString = "createLoan", LoanIdentifier = "abc" },
                new EntityAction() { ActionAsString = "setLoanField", LoanIdentifier = "abc", Field="loanAmount", Value = 100000 }
            };

            await processor.ProcessEntityActionsAsync(actionList.AsEnumerable());

            mockLoanRepository.LoanRepository.Count.Should().Be(1);
            var loan = mockLoanRepository.LoanRepository.First();
            loan.Id.Should().Be("abc");
            loan.LoanProperties.Count.Should().Be(1);
            var loanProperty = loan.LoanProperties.First();
            loanProperty.Name.Should().Be("loanAmount");
            loanProperty.ValueAsString.Should().Be("100000");
        }

        [Fact]
        public async Task ShouldProcess_SetBorrowerField()
        {
            var mockLoanRepository = new MockLoanRepository();
            var mockBorrowerRepository = new MockBorrowerRepository();
            var loggerMock = new Mock<ILogger<EntityActionProcessor>>();

            IEntityActionProcessor processor = new EntityActionProcessor(
                mockLoanRepository,
                mockBorrowerRepository,
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { ActionAsString = "createLoan", LoanIdentifier = "abc" },
                new EntityAction() { ActionAsString = "createBorrower", LoanIdentifier = "abc", BorrowerIdentifier = "def" },
                new EntityAction() { ActionAsString = "setBorrowerField", BorrowerIdentifier = "def", Field="firstName", Value = "Devon" }
            };

            await processor.ProcessEntityActionsAsync(actionList.AsEnumerable());

            mockLoanRepository.LoanRepository.Count.Should().Be(1);
            var loan = mockLoanRepository.LoanRepository.First();
            loan.Id.Should().Be("abc");

            mockBorrowerRepository.BorrowerRepository.Count.Should().Be(1);
            var borrower = mockBorrowerRepository.BorrowerRepository.First();
            borrower.Id.Should().Be("def");

            var borrowerProperty = borrower.BorrowerProperties.First();
            borrowerProperty.BorrowerId.Should().Be("def");
            borrowerProperty.Name.Should().Be("firstName");
            borrowerProperty.ValueAsString.Should().Be("Devon");
        }

        [Fact]
        public async Task ShouldProcess_Composite()
        {
            var mockLoanRepository = new MockLoanRepository();
            var mockBorrowerRepository = new MockBorrowerRepository();
            var loggerMock = new Mock<ILogger<EntityActionProcessor>>();

            IEntityActionProcessor processor = new EntityActionProcessor(
                mockLoanRepository,
                mockBorrowerRepository,
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { ActionAsString = "createLoan", LoanIdentifier = "abc" },
                new EntityAction() { ActionAsString = "createBorrower", LoanIdentifier = "abc", BorrowerIdentifier = "def" },
                new EntityAction() { ActionAsString = "createBorrower", LoanIdentifier = "abc", BorrowerIdentifier = "ggg" },
                new EntityAction() { ActionAsString = "setLoanField", LoanIdentifier = "abc", Field="loanAmount", Value = 100000 },
                new EntityAction() { ActionAsString = "setBorrowerField", BorrowerIdentifier = "def", Field="firstName", Value = "Devon" },
                new EntityAction() { ActionAsString = "setBorrowerField", BorrowerIdentifier = "def", Field="lastName", Value = "Yang" },
                new EntityAction() { ActionAsString = "setBorrowerField", BorrowerIdentifier = "ggg", Field="firstName", Value = "John" },
                new EntityAction() { ActionAsString = "setBorrowerField", BorrowerIdentifier = "ggg", Field="lastName", Value = "Smith" },
                new EntityAction() { ActionAsString = "setBorrowerField", BorrowerIdentifier = "ggg", Field="firstName", Value = null },
            };

            await processor.ProcessEntityActionsAsync(actionList.AsEnumerable());

            mockLoanRepository.LoanRepository.Count.Should().Be(1);
            var loan = mockLoanRepository.LoanRepository.First();
            loan.Id.Should().Be("abc");
            loan.Borrowers.Count.Should().Be(2);

            mockBorrowerRepository.BorrowerRepository.Count.Should().Be(2);
            var borrowerYang = mockBorrowerRepository.BorrowerRepository.First(x => x.Id == "def");
            borrowerYang.Id.Should().Be("def");
            borrowerYang.BorrowerProperties.Single(x => x.Name == "firstName").ValueAsString.Should().Be("Devon");
            borrowerYang.BorrowerProperties.Single(x => x.Name == "lastName").ValueAsString.Should().Be("Yang");

            var borrowerSmith = mockBorrowerRepository.BorrowerRepository.First(x => x.Id == "ggg");
            borrowerSmith.Id.Should().Be("ggg");
            borrowerSmith.BorrowerProperties.Single(x => x.Name == "firstName").ValueAsString.Should().BeNull();
            borrowerSmith.BorrowerProperties.Single(x => x.Name == "lastName").ValueAsString.Should().Be("Smith");
        }
    }
}