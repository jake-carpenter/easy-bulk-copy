using System;

namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithGuid))]
    public class TableWithGuid
    {
        public Guid ColumnName { get; set; }
    }
}