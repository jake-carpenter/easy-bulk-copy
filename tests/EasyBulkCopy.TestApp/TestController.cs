using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoBogus;
using Dapper;
using Microsoft.AspNetCore.Mvc;

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
            var records = AutoFaker.Generate<TestTable>(10);
            await _bulkInserter.Insert(_environment.ConnectionString, records);

            var results = new SqlConnection(_environment.ConnectionString)
                .Query<TestTable>("SELECT * FROM dbo.TestTable");

            if (results.Count() != 10)
                throw new Exception("Invalid insert");

            Console.WriteLine("Worked");
            return Ok();
        }
    }
}
