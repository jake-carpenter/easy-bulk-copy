namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithChar))]
    public class TableWithChar
    {
        public char ColumnName { get; set; }
    }
}