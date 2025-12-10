namespace Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateTestDatabaseAsync()
    {
        // Currently using SQL Server localdb for functional tests,
        // switch to another implementation if needed. Use SqlServerTestDatabase() for local db.
        var databse = new SqlTestcontainersTestDatabase();
        
        await databse.InitializeAsync();
        return databse;
    }
}
