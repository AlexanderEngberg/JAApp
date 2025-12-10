using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web.Infrastructure.Data;

namespace Application.FunctionalTests;

public class TestingFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static ITestDatabase _database = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseEnvironment("Testing")
            .UseSetting("ConnectionStrings:JAAppDb", _database.GetConnectionString());
        
        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                    options.UseSqlServer(_database.GetConnection());
                });
        });
    }

    public async ValueTask InitializeAsync()
    {
        _database = await TestDatabaseFactory.CreateTestDatabaseAsync();
    }

    public new async ValueTask DisposeAsync()
    {
        await _database.DisposeAsync();
        await base.DisposeAsync();
    }
}
