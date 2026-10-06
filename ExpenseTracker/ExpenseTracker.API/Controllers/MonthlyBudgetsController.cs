using ExpenseTracker.API.Application.MonthlyBudgets;
using ExpenseTracker.API.Domain.Entities;
using ExpenseTracker.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/monthly-budgets")]
public sealed class MonthlyBudgetsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public MonthlyBudgetsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPut("{year:int}/{month:int}")]
    public async Task<IActionResult> Upsert(
        int year,
        int month,
        UpsertMonthlyBudgetRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        if (!IsValidPeriod(year, month))
        {
            return ValidationProblem(ModelState);
        }

        var budget = await _dbContext.MonthlyBudgets
            .SingleOrDefaultAsync(
                budget => budget.Year == year && budget.Month == month,
                cancellationToken);

        if (budget is null)
        {
            budget = new MonthlyBudget(year, month, requestDto.Amount);
            _dbContext.MonthlyBudgets.Add(budget);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(
                nameof(GetStatus),
                new { year, month },
                new MonthlyBudgetDto(year, month, budget.Amount));
        }

        budget.UpdateAmount(requestDto.Amount);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpGet("{year:int}/{month:int}/status")]
    public async Task<ActionResult<MonthlyBudgetStatusDto>> GetStatus(
        int year,
        int month,
        CancellationToken cancellationToken)
    {
        if (!IsValidPeriod(year, month))
        {
            return ValidationProblem(ModelState);
        }

        var budget = await _dbContext.MonthlyBudgets
            .AsNoTracking()
            .SingleOrDefaultAsync(
                budget => budget.Year == year && budget.Month == month,
                cancellationToken);

        if (budget is null)
        {
            return NotFound();
        }

        var periodStart = new DateTime(
            year,
            month,
            1,
            0,
            0,
            0,
            DateTimeKind.Utc);
        var periodEnd = periodStart.AddMonths(1);

        var totalExpenses = await _dbContext.Expenses
            .Where(expense =>
                expense.Date >= periodStart &&
                expense.Date < periodEnd)
            .SumAsync(expense => expense.Amount, cancellationToken);

        var status = new MonthlyBudgetStatusDto(
            budget.Year,
            budget.Month,
            budget.Amount,
            totalExpenses,
            budget.Amount - totalExpenses,
            budget.IsExceededBy(totalExpenses));

        return Ok(status);
    }

    private bool IsValidPeriod(int year, int month)
    {
        if (year is < MonthlyBudget.MinYear or > MonthlyBudget.MaxYear)
        {
            ModelState.AddModelError(
                nameof(year),
                $"Year must be between {MonthlyBudget.MinYear} and {MonthlyBudget.MaxYear}.");
        }

        if (month is < 1 or > 12)
        {
            ModelState.AddModelError(
                nameof(month),
                "Month must be between 1 and 12.");
        }

        return ModelState.IsValid;
    }
}
