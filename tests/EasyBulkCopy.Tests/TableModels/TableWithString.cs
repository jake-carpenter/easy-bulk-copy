namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithString))]
    public class TableWithString
    {
        public string ColumnName { get; set; }
    }
}