using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.Application.MonthlyBudgets;

public sealed class UpsertMonthlyBudgetRequestDto
{
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal Amount { get; init; }
}
