namespace EasyBulkCopy.TestApp
{
    [BulkTableName("dbo.TestTable")]
    public class TestTable
    {
        public const string SqlToCreate = "create table TestTable (Id int, Name nvarchar(100))";
        public int Id { get; set; }
        public string Name { get; set; }
    }
}