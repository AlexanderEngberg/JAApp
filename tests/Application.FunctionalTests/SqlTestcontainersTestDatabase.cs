using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Respawn;
using Testcontainers.PostgreSql;
using Web.Infrastructure.Data;

namespace Application.FunctionalTests;

public class SqlTestcontainersTestDatabase : ITestDatabase
{
    private const string DefaultDatabase = "JAAppTestDb";
    private readonly PostgreSqlContainer _container;
    private DbConnection _connection = null!;
    private string _connectionString = null!;
    private Respawner _respawner = null!;

    public SqlTestcontainersTestDatabase()
    {
        _container = new PostgreSqlBuilder()
            .WithAutoRemove(true)
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await _container.ExecScriptAsync($"CREATE DATABSE {DefaultDatabase}");

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_container.GetConnectionString())
        {
            InitialCatalog = DefaultDatabase
        };

        _connectionString = builder.ConnectionString;

        _connection = new SqlConnection(_connectionString);

        var option = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_connectionString)
            .ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning))
            .Options;

        ApplicationDbContext context = new ApplicationDbContext(option);

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
        await _container.DisposeAsync();
    }
}
