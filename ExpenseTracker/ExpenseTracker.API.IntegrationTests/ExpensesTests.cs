using System.Net;
using System.Net.Http.Json;
using ExpenseTracker.API.Application.Expenses;
using ExpenseTracker.API.Domain.Entities;
using ExpenseTracker.API.IntegrationTests.Infrustructure;

namespace ExpenseTracker.API.IntegrationTests;

public sealed class ExpensesTests : IClassFixture<ExpenseTrackerFixture>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly ExpenseTrackerFixture _fixture;

    public ExpensesTests(ExpenseTrackerFixture fixture)
    {
        _fixture = fixture;
        
        _client = fixture.Client
                  ?? throw new InvalidOperationException(
                      "Test HTTP client has not been initialized.");
    }
    
    public Task InitializeAsync()
    {
        return _fixture.ResetDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    
    [Fact]
    public async Task Create_WhenRequestIsValid_ExpenseAppearsInList()
    {
        // Arrange
        var request = new ExpenseRequestDto
        {
            Description = "Lunch",
            Amount = 2_500m,
            Date = new DateTime(2026, 10, 14, 12, 0, 0, DateTimeKind.Utc),
            Category = "Food"
        };

        // Act
        using var createResponse =
            await _client.PostAsJsonAsync("/api/expenses", request);

        // Assert: creation
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<Expense>();

        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created.Id);

        // Assert: get saved expense
        using var listResponse = await _client.GetAsync("/api/expenses");

        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        var expenses =
            await listResponse.Content.ReadFromJsonAsync<List<Expense>>();

        Assert.NotNull(expenses);

        var saved = Assert.Single(
            expenses,
            expense => expense.Id == created.Id);

        Assert.Equal(request.Description, saved.Description);
        Assert.Equal(request.Amount, saved.Amount);
        Assert.Equal(request.Date, saved.Date);
        Assert.Equal(request.Category, saved.Category);
    }
    
    [Fact]
    public async Task GetAll_WhenNoExpenses_ReturnsEmptyList()
    {
        using var response = await _client.GetAsync("/api/expenses");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var expenses =
            await response.Content.ReadFromJsonAsync<List<Expense>>();

        Assert.NotNull(expenses);
        Assert.Empty(expenses);
    }
}