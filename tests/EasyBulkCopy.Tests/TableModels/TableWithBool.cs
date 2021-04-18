namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithBool))]
    public class TableWithBool
    {
        public const string SqlToCreate = "create table TableWithBool (ColumnName bit)";
        public bool ColumnName { get; set; }
    }
}