using System.Data.SqlClient;

namespace EasyBulkCopy.TestApp
{
    [BulkTableName("dbo.TestTable", SqlBulkCopyOptions.KeepIdentity)]
    public class TestTable
    {
        public const string SqlToCreate = @"
            if object_id('dbo.TestTable', 'U') is null begin
                create table TestTable (Id int, Name nvarchar(100));
            end;";

        public int Id { get; set; }
        public string Name { get; set; }
    }
}