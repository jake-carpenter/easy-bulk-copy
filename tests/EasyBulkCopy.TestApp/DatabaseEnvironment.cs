using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Mssql;

namespace EasyBulkCopy.TestApp
{
    public class DatabaseEnvironment : IDisposable
    {
        public const string DbName = "TestAppDb";
        public const string DbPassword = "p4$4w0rd";
        private readonly DockerEnvironment _environment;

        public DatabaseEnvironment()
        {
            _environment = new DockerEnvironmentBuilder()
                .AddMssqlContainer(DbName, DbPassword)
                .Build();

            _environment.Up().Wait();
            Container = _environment.GetContainer<MssqlContainer>(DbName);

            var port = Container.Ports.Values.First();
            var connectionString = $"Server=localhost,{port};Database=master;User Id=sa;Password={DbPassword}";
            using var connection = GetConnection(connectionString);
            CreateTestTables(connection).Wait();
        }
        
        public MssqlContainer Container { get; }

        public void Dispose()
        {
            _environment?.Dispose();
        }
        
        private static SqlConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        private static Task CreateTestTables(SqlConnection connection)
        {
            return connection.ExecuteAsync(TestTable.SqlToCreate);
        }
    }
}