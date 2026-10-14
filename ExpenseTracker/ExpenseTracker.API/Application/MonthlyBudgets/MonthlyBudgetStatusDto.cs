namespace ExpenseTracker.API.Application.MonthlyBudgets;

public sealed record MonthlyBudgetStatusDto(
    int Year,
    int Month,
    decimal BudgetAmount,
    decimal TotalExpenses,
    decimal Difference,
    bool IsExceeded);
