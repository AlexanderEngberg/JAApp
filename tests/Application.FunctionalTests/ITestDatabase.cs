using System.Data.Common;

namespace Application.FunctionalTests;

public interface ITestDatabase
{
    Task InitializeAsync();

    DbConnection GetConnection();

    string GetConnectionString();

    Task ResetAsync();

    Task DisposeAsync();
}