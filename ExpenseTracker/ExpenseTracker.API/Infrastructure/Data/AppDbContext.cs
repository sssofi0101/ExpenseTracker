using ExpenseTracker.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; }
}
