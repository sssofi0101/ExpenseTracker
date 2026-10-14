using ExpenseTracker.API.Domain.Entities;
using Xunit;

namespace ExpenseTracker.API.Tests.Domain.Entities;

public sealed class MonthlyBudgetTests
{
    [Theory]
    [InlineData(1_000, 999.99, false)]
    [InlineData(1_000, 1_000, false)]
    [InlineData(1_000, 1_000.01, true)]
    public void IsExceededBy_ReturnsExpectedResult(
        decimal budgetAmount,
        decimal totalExpenses,
        bool expected)
    {
        var budget = new MonthlyBudget(2026, 10, budgetAmount);

        var result = budget.IsExceededBy(totalExpenses);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_WhenAmountIsNegative_ThrowsArgumentOutOfRangeException()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new MonthlyBudget(2026, 10, -0.01m));

        Assert.Equal("amount", exception.ParamName);
    }

    [Theory]
    [InlineData(1979)]
    [InlineData(2101)]
    public void Constructor_WhenYearIsOutsideSupportedRange_ThrowsArgumentOutOfRangeException(
        int year)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new MonthlyBudget(year, 10, 1_000m));

        Assert.Equal("year", exception.ParamName);
    }

    [Fact]
    public void IsExceededBy_WhenTotalExpensesAreNegative_ThrowsArgumentOutOfRangeException()
    {
        var budget = new MonthlyBudget(2026, 10, 1_000m);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => budget.IsExceededBy(-0.01m));

        Assert.Equal("totalExpenses", exception.ParamName);
    }
}
