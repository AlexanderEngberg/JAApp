using System.Data.Common;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Testcontainers.PostgreSql;
using Web.Infrastructure.Data;

namespace Application.FunctionalTests;

public class PostgresSQLTestcontainersTestDatabase : ITestDatabase
{
    private const string DefaultDatabase = "JAAppTestDb";
    private readonly PostgreSqlContainer _container;
    private DbConnection _connection = null!;
    private string _connectionString = null!;
    private Respawner _respawner = null!;

    public PostgresSQLTestcontainersTestDatabase()
    {
        _container = new PostgreSqlBuilder()
            .WithAutoRemove(true)
            .Build();
    }
    
    public async Task InitializeAsync()
    {

        await _container.StartAsync();
        await _container.ExecScriptAsync($"CREATE DATABASE {DefaultDatabase}");

        var builder = new NpgsqlConnectionStringBuilder(_container.GetConnectionString())
        {
            Database = DefaultDatabase
        };

        _connectionString = builder.ConnectionString;

        _connection = new NpgsqlConnection(_connectionString);

        var option = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_connectionString)
            .Options;

        ApplicationDbContext context = new ApplicationDbContext(option);

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        await _connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres
        });
        await _connection.CloseAsync();
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
        await _connection.OpenAsync();
        await _respawner.ResetAsync(_connection);
        await _connection.CloseAsync();
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _container.DisposeAsync();
    }
}
