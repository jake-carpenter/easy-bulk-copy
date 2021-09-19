using System;

namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithGuid))]
    public class TableWithGuid
    {
        public const string SqlToCreate = @"
            if object_id('dbo.TableWithGuid', 'U') is null begin
                create table dbo.TableWithGuid (ColumnName uniqueidentifier);
            end;";

        public Guid ColumnName { get; set; }
    }
}