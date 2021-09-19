using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;

namespace EasyBulkCopy.TestApp
{
    public class DatabaseEnvironment
    {
        public DatabaseEnvironment(DatabaseConfig config)
        {
            var container = new Builder()
                .UseContainer()
                .UseImage("mcr.microsoft.com/mssql/server:2019-latest")
                .WithEnvironment("ACCEPT_EULA=Y", $"SA_PASSWORD={config.Password}")
                .WithName("easy-bulk-copy-test-app-db")
                .ReuseIfExists()
                .ExposePort(config.Port, 1433)
                .Build();

            if (container.State != ServiceRunningState.Running)
            {
                container.Start();
                container.WaitForMessageInLogs("The tempdb database has", 30000);
            }

            ConnectionString = $"Server=localhost,{config.Port};Database=master;User Id=sa;Password={config.Password}";
            using var connection = GetConnection(ConnectionString);
            CreateTestTables(connection).Wait();
        }

        public string ConnectionString { get; }

        private static SqlConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        private static Task CreateTestTables(IDbConnection connection)
        {
            return connection.ExecuteAsync(TestTable.SqlToCreate);
        }
    }
}
