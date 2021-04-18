namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithNullableBool))]
    public class TableWithNullableBool
    {
        public bool? ColumnName { get; set; }
    }
}