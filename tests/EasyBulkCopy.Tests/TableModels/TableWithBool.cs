namespace EasyBulkCopy.Tests.TableModels
{
    [BulkTableName(nameof(TableWithBool))]
    public class TableWithBool
    {
        public const string SqlToCreate = @"
            if object_id('dbo.TableWithBool', 'U') is null begin
                create table dbo.TableWithBool (ColumnName bit);
            end;";
        public bool ColumnName { get; set; }
    }
}