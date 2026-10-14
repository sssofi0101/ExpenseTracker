namespace ExpenseTracker.API.Domain.Entities;

public sealed class MonthlyBudget
{
    public const int MinYear = 1980;
    public const int MaxYear = 2100;

    private MonthlyBudget()
    {
    }

    public MonthlyBudget(int year, int month, decimal amount)
    {
        if (year is < MinYear or > MaxYear)
        {
            throw new ArgumentOutOfRangeException(
                nameof(year),
                year,
                $"Year must be between {MinYear} and {MaxYear}.");
        }

        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(
                nameof(month),
                month,
                "Month must be between 1 and 12.");
        }

        Id = Guid.NewGuid();
        Year = year;
        Month = month;
        UpdateAmount(amount);
    }

    public Guid Id { get; private set; }
    public int Year { get; private set; }
    public int Month { get; private set; }
    public decimal Amount { get; private set; }

    public void UpdateAmount(decimal amount)
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
