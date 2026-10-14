namespace ExpenseTracker.API.Application.MonthlyBudgets;

public sealed record class MonthlyBudgetDto(
    int Year,
    int Month,
    decimal Amount);
