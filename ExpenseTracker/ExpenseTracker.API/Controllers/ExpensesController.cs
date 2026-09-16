using ExpenseTracker.API.Application.Expenses;
using ExpenseTracker.API.Domain.Entities;
using ExpenseTracker.API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/expenses")]
public sealed class ExpensesController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ExpensesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<Expense>> Create(
        ExpenseRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Description = requestDto.Description,
            Amount = requestDto.Amount,
            Date = requestDto.Date,
            Category = requestDto.Category
        };

        _dbContext.Expenses.Add(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Expense>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Expenses
            .AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id, cancellationToken);

        return expense is null ? NotFound() : Ok(expense);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<Expense>>> GetAll(
        CancellationToken cancellationToken)
    {
        var expenses = await _dbContext.Expenses
            .AsNoTracking()
            .OrderByDescending(expense => expense.Date)
            .ToListAsync(cancellationToken);

        return Ok(expenses);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        ExpenseRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Expenses
            .FirstOrDefaultAsync(expense => expense.Id == id, cancellationToken);

        if (expense is null)
        {
            return NotFound();
        }

        expense.Description = requestDto.Description;
        expense.Amount = requestDto.Amount;
        expense.Date = requestDto.Date;
        expense.Category = requestDto.Category;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Expenses
            .FirstOrDefaultAsync(expense => expense.Id == id, cancellationToken);

        if (expense is null)
        {
            return NotFound();
        }

        _dbContext.Expenses.Remove(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
