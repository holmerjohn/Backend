using Backend.Converters;
using Backend.Domain;
using Backend.Enums;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace Backend.Tests
{
    public class LoanActionTests
    {
        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
                {
                    new EntityActionTypeJsonConverter()
                }
        };

        [Fact]
        public void ShouldCreate_CreateLoan()
        {
            var inputString = JsonSerializer.Serialize(new { action = "createLoan", loanIdentifier = "abc" });
            var outputAction = JsonSerializer.Deserialize<EntityAction>(inputString, _options);

            outputAction.Should().NotBeNull();
            outputAction.Action.Should().Be(EntityActionType.CreateLoan);
            outputAction.LoanIdentifier.Should().Be("abc");
        }

        [Fact]
        public void ShouldCreate_CreateBorrower()
        {
            var inputString = JsonSerializer.Serialize(new { action = "createBorrower", loanIdentifier = "abc", borrowerIdentifier = "def" });
            var outputAction = JsonSerializer.Deserialize<EntityAction>(inputString, _options);

            outputAction.Should().NotBeNull();
            outputAction.Action.Should().Be(EntityActionType.CreateBorrower);
            outputAction.LoanIdentifier.Should().Be("abc");
            outputAction.BorrowerIdentifier.Should().Be("def");
        }

        [Fact]
        public void ShouldCreate_SetLoanField()
        {
            var inputString = JsonSerializer.Serialize(new { action = "setLoanField", loanIdentifier = "abc", field = "loanAmount", value = 100000 });
            var outputAction = JsonSerializer.Deserialize<EntityAction>(inputString, _options);

            outputAction.Should().NotBeNull();
            outputAction.Action.Should().Be(EntityActionType.SetLoanField);
            outputAction.LoanIdentifier.Should().Be("abc");
            outputAction.Field.Should().Be("loanAmount");
            outputAction.Value.Should().Be(100000);
        }

        [Fact]
        public void ShouldCreate_SetBorrowerField()
        {
            var inputString = JsonSerializer.Serialize(new { action = "setBorrowerField", borrowerIdentifier = "def", field = "firstName", value = "Devon" });
            var outputAction = JsonSerializer.Deserialize<EntityAction>(inputString, _options);

            outputAction.Should().NotBeNull();
            outputAction.Action.Should().Be(EntityActionType.SetBorrowerField);
            outputAction.BorrowerIdentifier.Should().Be("def");
            outputAction.Field.Should().Be("firstName");
            outputAction.Value.Should().Be("Devon");
        }
    }
}