using System;

namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithDateTime))]
    public class TableWithDateTime
    {
        public DateTime ColumnName { get; set; }
    }
}