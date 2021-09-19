using System;

namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithManyColumns))]
    public class TableWithManyColumns
    {
        public const string SqlToCreate = @"
            if object_id('dbo.TableWithManyColumns', 'U') is null begin
                create table dbo.TableWithManyColumns (
                    Id int, Name nvarchar(100), ForeignKey uniqueidentifier, Created datetimeoffset, IsActive bit
                );
            end;";

        public int Id { get; set; }
        public string Name { get; set; }
        public Guid? ForeignKey { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool IsActive { get; set; }
    }
}
