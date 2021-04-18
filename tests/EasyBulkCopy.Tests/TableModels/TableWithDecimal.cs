namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithDecimal))]
    public class TableWithDecimal
    {
        public decimal ColumnName { get; set; }
    }
}