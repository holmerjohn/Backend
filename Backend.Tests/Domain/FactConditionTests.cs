using Backend.Converters;
using Backend.Domain;
using Backend.Domain.Facts;
using Backend.Domain.Loans;
using Backend.Enums;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace Backend.Tests.Domain
{
    public class FactConditionTests
    {
        [Fact]
        public void ShouldSatisfyCondition_NoProperty()
        {
            var condition = new FactCondition() { Field = "firstName", PropertyType = PropertyType.String, Comparator = ConditionComparator.Exists };
            condition.IsSatisfiedByProperty(null).Should().BeFalse();
        }

        [Fact]
        public void ShouldSatisfyCondition_Exists()
        {
            var property = new BorrowerProperty() { Name = "fistName", PropertyType = PropertyType.String, StringValue = "Devon" };
            var condition = new FactCondition() { Field = "firstName", PropertyType = PropertyType.String, Comparator = ConditionComparator.Exists };
            condition.IsSatisfiedByProperty(property).Should().BeTrue();
        }

        [Fact]
        public void ShouldSatisfyCondition_NotExists()
        {
            var property = new BorrowerProperty() { Name = "fistName", PropertyType = PropertyType.Null, StringValue = null };
            var condition = new FactCondition() { Field = "firstName", PropertyType = PropertyType.String, Comparator = ConditionComparator.Exists };
            condition.IsSatisfiedByProperty(property).Should().BeFalse();
        }

        [Fact]
        public void ShouldSatisfyCondition_EqualsString()
        {
            var property = new BorrowerProperty() { Name = "fistName", PropertyType = PropertyType.String, StringValue = "Devon" };
            var condition = new FactCondition() { Field = "firstName", PropertyType = PropertyType.String, Comparator = ConditionComparator.Equals, Value="Devon" };
            condition.IsSatisfiedByProperty(property).Should().BeTrue();
        }

        [Fact]
        public void ShouldSatisfyCondition_NotEqualsString()
        {
            var property = new BorrowerProperty() { Name = "fistName", PropertyType = PropertyType.String, StringValue = "Devon" };
            var condition = new FactCondition() { Field = "firstName", PropertyType = PropertyType.String, Comparator = ConditionComparator.Equals, Value = "Frank" };
            condition.IsSatisfiedByProperty(property).Should().BeFalse();
        }


        [Fact]
        public void ShouldSatisfyCondition_EqualsNumber()
        {
            var property = new LoanProperty() { Name = "loanAmount", PropertyType = PropertyType.Number, NumberValue = 100000 };
            var condition = new FactCondition() { Field = "loanAmount", PropertyType = PropertyType.Number, Comparator = ConditionComparator.Equals, Value = "100000" };
            condition.IsSatisfiedByProperty(property).Should().BeTrue();
        }

        [Fact]
        public void ShouldSatisfyCondition_NotEqualsNumber()
        {
            var property = new LoanProperty() { Name = "loanAmount", PropertyType = PropertyType.Number, NumberValue = 1 };
            var condition = new FactCondition() { Field = "loanAmount", PropertyType = PropertyType.Number, Comparator = ConditionComparator.Equals, Value = "100000" };
            condition.IsSatisfiedByProperty(property).Should().BeFalse();
        }

    }
}
