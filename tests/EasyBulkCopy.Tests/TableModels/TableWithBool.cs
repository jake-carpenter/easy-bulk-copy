namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithBool))]
    public class TableWithBool
    {
        public string ColumnName { get; set; }
    }
}