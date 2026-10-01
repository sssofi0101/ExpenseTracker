namespace ExpenseTracker.API.Domain.Entities;

public sealed class MonthlyBudget
{
    public MonthlyBudget(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                amount,
                "Monthly budget cannot be negative.");
        }

        Amount = amount;
    }

    public decimal Amount { get; }

    public bool IsExceededBy(decimal totalExpenses)
    {
        if (totalExpenses < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalExpenses),
                totalExpenses,
                "Total expenses cannot be negative.");
        }

        return totalExpenses > Amount;
    }
}
