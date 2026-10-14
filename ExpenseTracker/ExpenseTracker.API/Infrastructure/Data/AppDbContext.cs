using ExpenseTracker.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.API.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<MonthlyBudget> MonthlyBudgets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MonthlyBudget>()
            .HasIndex(budget => new { budget.Year, budget.Month })
            .IsUnique();
    }
}
