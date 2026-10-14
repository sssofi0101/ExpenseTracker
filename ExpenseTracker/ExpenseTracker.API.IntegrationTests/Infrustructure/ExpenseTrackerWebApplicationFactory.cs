using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExpenseTracker.API.IntegrationTests.Infrustructure;

public sealed class ExpenseTrackerWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public ExpenseTrackerWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseSetting(
            "ConnectionStrings:DefaultConnection",
            _connectionString);
    }
}