namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithLong))]
    public class TableWithLong
    {
        public long ColumnName { get; set; }
    }
}