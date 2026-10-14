using ExpenseTracker.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace ExpenseTracker.API.IntegrationTests.Infrustructure;

public sealed class ExpenseTrackerFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder("postgres:18")
            .WithDatabase("expense_tracker_tests")
            .WithUsername("postgres")
            .WithPassword("test_password")
            .Build();
    
    public ExpenseTrackerWebApplicationFactory? Factory { get; private set; }
    public HttpClient? Client { get; private set; }
    
    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        
        Factory = new ExpenseTrackerWebApplicationFactory(_postgres.GetConnectionString());
        Client = Factory?.CreateClient();
    }

    public async Task DisposeAsync()
    {
        try
        {
            Client?.Dispose();

            if (Factory is not null)
            {
                await Factory.DisposeAsync();
            }
        }
        finally
        {
            await _postgres.DisposeAsync();
        }
    }
    
    public async Task ResetDatabaseAsync()
    {
        var factory = Factory
                      ?? throw new InvalidOperationException(
                          "Test application has not been initialized.");

        await using var scope = factory.Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        await dbContext.Expenses.ExecuteDeleteAsync();
        await dbContext.MonthlyBudgets.ExecuteDeleteAsync();
    }
}