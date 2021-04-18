using System;

namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithGuid))]
    public class TableWithGuid
    {
        public const string SqlToCreate = "create table TableWithGuid (ColumnName uniqueidentifier)";

        public Guid ColumnName { get; set; }
    }
}