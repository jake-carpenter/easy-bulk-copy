using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EasyBulkCopy.Tests.TableModels;
using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Mssql;
using Xunit;

namespace EasyBulkCopy.Tests
{
    public class DatabaseEnvironmentFixture : IAsyncLifetime
    {
        private const string DbName = "TestDb";
        private const string DbPassword = "p4$4w0rd";
        private readonly DockerEnvironment _environment;
        private SqlConnection _connection;
        private MssqlContainer _container;

        public DatabaseEnvironmentFixture()
        {
            _environment = new DockerEnvironmentBuilder()
                .AddMssqlContainer(DbName, DbPassword)
                .Build();
        }

        public string ConnectionString { get; private set; }

        public List<T> GetAllRecordsInTable<T>(string tableName)
        {
            return _connection.Query<T>($"select * from {tableName}").ToList();
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _environment.Up();
                _container = _environment.GetContainer<MssqlContainer>(DbName);

                var port = _container.Ports.Values.First();
                ConnectionString = $"Server=localhost,{port};Database=master;User Id=sa;Password={DbPassword}";
                _connection = GetConnection(ConnectionString);
                await CreateTestTables(_connection);
            }
            catch
            {
                DisposeAsync().Wait();
                throw;
            }
        }

        public async Task ClearTables()
        {
            await _connection.ExecuteAsync("drop table TableWithManyColumns");
            await _connection.ExecuteAsync("drop table TableWithBool");
            await _connection.ExecuteAsync("drop table TableWithGuid");
            await CreateTestTables(_connection);
        }

        public async Task DisposeAsync()
        {
            _connection?.Dispose();
            if (_environment != null)
            {
                await _environment.Down();
                await _environment.DisposeAsync();
            }
        }

        private static SqlConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        private static async Task CreateTestTables(SqlConnection connection)
        {
            await connection.ExecuteAsync(TableWithManyColumns.SqlToCreate);
            await connection.ExecuteAsync(TableWithBool.SqlToCreate);
            await connection.ExecuteAsync(TableWithGuid.SqlToCreate);
        }
    }
}