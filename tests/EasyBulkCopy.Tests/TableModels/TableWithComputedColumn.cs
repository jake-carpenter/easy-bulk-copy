namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithComputedColumn))]
    public class TableWithComputedColumn
    {
        public const string SqlToCreate = @"
            if object_id('dbo.TableWithComputedColumn', 'U') is null begin
                create table dbo.TableWithComputedColumn (
                    Id int, IdIsEven AS IIF(Id % 2 = 0, 1, 0)
                );
            end;";

        public int Id { get; set; }

        [Computed]
        public bool IdIsEven { get; set; }
    }
}
