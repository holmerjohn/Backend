using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Backend.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Backend.Enums;
using Backend.Program.Tests.MockRepositories;

namespace Backend.Program.Tests
{
    public class ActionProcessorTests
    {
        [Fact]
        public async Task ShouldProcess_CreateLoan()
        {
            var mockLoanRepository = new MockLoanRepository();
            var factEngineMock = new Mock<IFactEngine>();
            var loggerMock = new Mock<ILogger<LoanActionProcessor>>();
            

            ILoanActionProcessor processor = new LoanActionProcessor(
                mockLoanRepository,
                new MockBorrowerRepository(),
                factEngineMock.Object,
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { Action = EntityActionType.CreateLoan, LoanIdentifier = "abc" }
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
            var factEngineMock = new Mock<IFactEngine>();
            var loggerMock = new Mock<ILogger<LoanActionProcessor>>();

            ILoanActionProcessor processor = new LoanActionProcessor(
                mockLoanRepository,
                mockBorrowerRepository,
                factEngineMock.Object,
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { Action = EntityActionType.CreateLoan, LoanIdentifier = "abc" },
                new EntityAction() { Action = EntityActionType.CreateBorrower, LoanIdentifier = "abc", BorrowerIdentifier = "def" }
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
            var factEngineMock = new Mock<IFactEngine>();
            var loggerMock = new Mock<ILogger<LoanActionProcessor>>();

            ILoanActionProcessor processor = new LoanActionProcessor(
                mockLoanRepository,
                new MockBorrowerRepository(),
                factEngineMock.Object,
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { Action = EntityActionType.CreateLoan, LoanIdentifier = "abc" },
                new EntityAction() { Action = EntityActionType.SetLoanField, LoanIdentifier = "abc", Field="loanAmount", Value = 100000 }
            };

            await processor.ProcessEntityActionsAsync(actionList.AsEnumerable());

            mockLoanRepository.LoanRepository.Count.Should().Be(1);
            var loan = mockLoanRepository.LoanRepository.First();
            loan.Id.Should().Be("abc");
            loan.LoanProperties.Count.Should().Be(1);

            var loanProperty = loan.LoanProperties.First();
            loanProperty.Name.Should().Be("loanAmount");
            loanProperty.NumberValue.Should().Be(100000);
            loanProperty.PropertyType.Should().Be(PropertyType.Number);
        }

        [Fact]
        public async Task ShouldProcess_SetBorrowerField()
        {
            var mockLoanRepository = new MockLoanRepository();
            var mockBorrowerRepository = new MockBorrowerRepository();
            var factEngineMock = new Mock<IFactEngine>();
            var loggerMock = new Mock<ILogger<LoanActionProcessor>>();

            ILoanActionProcessor processor = new LoanActionProcessor(
                mockLoanRepository,
                mockBorrowerRepository,
                factEngineMock.Object,
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { Action = EntityActionType.CreateLoan, LoanIdentifier = "abc" },
                new EntityAction() { Action = EntityActionType.CreateBorrower, LoanIdentifier = "abc", BorrowerIdentifier = "def" },
                new EntityAction() { Action = EntityActionType.SetBorrowerField, BorrowerIdentifier = "def", Field="firstName", Value = "Devon" }
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
            borrowerProperty.StringValue.Should().Be("Devon");
        }

        [Fact]
        public async Task ShouldProcess_Composite()
        {
            var mockLoanRepository = new MockLoanRepository();
            var mockBorrowerRepository = new MockBorrowerRepository();
            var factEngineMock = new Mock<IFactEngine>();
            var loggerMock = new Mock<ILogger<LoanActionProcessor>>();

            ILoanActionProcessor processor = new LoanActionProcessor(
                mockLoanRepository,
                mockBorrowerRepository,
                factEngineMock.Object,
                loggerMock.Object,
                new BackendConfiguration());

            var actionList = new List<EntityAction>()
            {
                new EntityAction() { Action = EntityActionType.CreateLoan, LoanIdentifier = "abc" },
                new EntityAction() { Action = EntityActionType.CreateBorrower, LoanIdentifier = "abc", BorrowerIdentifier = "def" },
                new EntityAction() { Action = EntityActionType.CreateBorrower, LoanIdentifier = "abc", BorrowerIdentifier = "ggg" },
                new EntityAction() { Action = EntityActionType.SetLoanField, LoanIdentifier = "abc", Field="loanAmount", Value = 100000 },
                new EntityAction() { Action = EntityActionType.SetBorrowerField, BorrowerIdentifier = "def", Field="firstName", Value = "Devon" },
                new EntityAction() { Action = EntityActionType.SetBorrowerField, BorrowerIdentifier = "def", Field="lastName", Value = "Yang" },
                new EntityAction() { Action = EntityActionType.SetBorrowerField, BorrowerIdentifier = "ggg", Field="firstName", Value = "John" },
                new EntityAction() { Action = EntityActionType.SetBorrowerField, BorrowerIdentifier = "ggg", Field="lastName", Value = "Smith" },
                new EntityAction() { Action = EntityActionType.SetBorrowerField, BorrowerIdentifier = "ggg", Field="firstName", Value = null },
            };

            await processor.ProcessEntityActionsAsync(actionList.AsEnumerable());

            mockLoanRepository.LoanRepository.Count.Should().Be(1);
            var loan = mockLoanRepository.LoanRepository.First();
            loan.Id.Should().Be("abc");
            loan.Borrowers.Count.Should().Be(2);

            mockBorrowerRepository.BorrowerRepository.Count.Should().Be(2);
            var borrowerYang = mockBorrowerRepository.BorrowerRepository.First(x => x.Id == "def");
            borrowerYang.Id.Should().Be("def");
            borrowerYang.Loans.Count.Should().Be(1);
            borrowerYang.BorrowerProperties.Single(x => x.Name == "firstName").StringValue.Should().Be("Devon");
            borrowerYang.BorrowerProperties.Single(x => x.Name == "lastName").StringValue.Should().Be("Yang");

            var borrowerSmith = mockBorrowerRepository.BorrowerRepository.First(x => x.Id == "ggg");
            borrowerSmith.Id.Should().Be("ggg");
            borrowerSmith.Loans.Count.Should().Be(1);
            borrowerSmith.BorrowerProperties.Single(x => x.Name == "firstName").StringValue.Should().BeNull();
            borrowerSmith.BorrowerProperties.Single(x => x.Name == "lastName").StringValue.Should().Be("Smith");
        }
    }
}