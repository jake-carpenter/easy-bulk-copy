namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithInt))]
    public class TableWithInt
    {
        public int ColumnName { get; set; }
    }
}