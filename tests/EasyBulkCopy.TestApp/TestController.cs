using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoBogus;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Mssql;

namespace EasyBulkCopy.TestApp
{
    public class TestController : ControllerBase
    {
        private readonly IBulkInserter _bulkInserter;
        private readonly DatabaseEnvironment _environment;

        public TestController(IBulkInserter bulkInserter, DatabaseEnvironment environment)
        {
            _bulkInserter = bulkInserter;
            _environment = environment;
        }

        [HttpGet("/test")]
        public async Task<IActionResult> Test()
        {
            var port = _environment.Container.Ports.Values.First();
            var connectionString =
                $"Server=localhost,{port};Database=master;User Id=sa;Password={DatabaseEnvironment.DbPassword}";


            var records = AutoFaker.Generate<TestTable>(10);
            await _bulkInserter.Insert(connectionString, records);

            var results = new SqlConnection(connectionString)
                .Query<TestTable>("SELECT * FROM dbo.TestTable");

            if (results.Count() != 10)
                throw new Exception("Invalid insert");

            Console.WriteLine("Worked");
            return Ok();
        }
    }
}