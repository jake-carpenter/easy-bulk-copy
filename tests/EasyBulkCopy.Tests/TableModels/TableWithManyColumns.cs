using System;

namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithManyColumns))]
    public class TableWithManyColumns
    {
        public const string SqlToCreate = @"
            create table TableWithManyColumns (
                Id int, Name nvarchar(100), ForeignKey uniqueidentifier, created datetimeoffset, IsActive bit
            )";
        
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid? ForeignKey { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool IsActive { get; set; }
    }
}