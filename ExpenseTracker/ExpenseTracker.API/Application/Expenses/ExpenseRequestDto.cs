using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.Application.Expenses;

public sealed class ExpenseRequestDto
{
    [Required]
    [MaxLength(500)]
    public string Description { get; init; } = string.Empty;

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Amount { get; init; }

    public DateTime Date { get; init; }

    [Required]
    [MaxLength(200)]
    public string Category { get; init; } = string.Empty;
}
