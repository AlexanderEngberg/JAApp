using System.Data.Common;
using Ardalis.GuardClauses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Respawn;
using Web.Infrastructure.Data;

namespace Application.FunctionalTests;

internal class SqlServerTestDatabase : ITestDatabase
{
    private readonly string _connectionString = null!;

    private SqlConnection _connection = null!;

    private Respawner _respawner = null!;

    public SqlServerTestDatabase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        
        var connectionString = configuration.GetConnectionString("JAAppDb");

        Guard.Against.Null(connectionString);

        _connectionString = connectionString;
    }

    public async Task InitializeAsync()
    {
        _connection = new SqlConnection(_connectionString);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connection)
            .Options;
        
        var context = new ApplicationDbContext(options);

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        _respawner = await Respawner.CreateAsync(_connectionString);
    }

    public DbConnection GetConnection()
    {
        return _connection;
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(_connectionString);
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}