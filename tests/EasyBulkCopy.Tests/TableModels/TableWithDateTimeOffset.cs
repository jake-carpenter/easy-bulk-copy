using System;

namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithDateTimeOffset))]
    public class TableWithDateTimeOffset
    {
        public DateTimeOffset ColumnName { get; set; }
    }
}