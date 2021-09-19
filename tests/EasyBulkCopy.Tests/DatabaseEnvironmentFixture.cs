using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using EasyBulkCopy.Tests.TableModels;
using Xunit;

namespace EasyBulkCopy.Tests
{
    public class DatabaseEnvironmentFixture : IAsyncLifetime
    {
        private readonly IContainerService _container;
        private SqlConnection _connection;

        public DatabaseEnvironmentFixture()
        {
            _container = new Builder()
                .UseContainer()
                .UseImage("mcr.microsoft.com/mssql/server:2019-latest")
                .KeepContainer()
                .KeepRunning()
                .WithEnvironment("ACCEPT_EULA=Y", $"SA_PASSWORD=Password123")
                .WithName("easy-bulk-copy-test-db")
                .ReuseIfExists()
                .ExposePort(15232, 1433)
                .Build();
        }

        public string ConnectionString { get; private set; }

        public async Task InitializeAsync()
        {
            try
            {
                if (_container.State != ServiceRunningState.Running)
                {
                    _container.Start();
                    _container.WaitForMessageInLogs("The tempdb database has", 30000);
                }


                ConnectionString = $"Server=localhost,15232;Database=master;User Id=sa;Password=Password123";
                _connection = GetConnection(ConnectionString);
                await CreateTestTables(_connection);
            }
            catch
            {
                await DisposeAsync();
                throw;
            }
        }

        public Task DisposeAsync()
        {
            _connection?.Dispose();

            return Task.CompletedTask;
        }

        public List<T> GetAllRecordsInTable<T>(string tableName)
        {
            return _connection.Query<T>($"select * from {tableName}").ToList();
        }

        public async Task ClearTables()
        {
            await _connection.ExecuteAsync("drop table TableWithManyColumns");
            await _connection.ExecuteAsync("drop table TableWithBool");
            await _connection.ExecuteAsync("drop table TableWithGuid");
            await _connection.ExecuteAsync("drop table TableWithComputedColumn");
            await CreateTestTables(_connection);
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
            await connection.ExecuteAsync(TableWithComputedColumn.SqlToCreate);
        }
    }
}
