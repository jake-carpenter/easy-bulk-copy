using System.Linq;
using AutoBogus;
using EasyBulkCopy.Tests.TableModels;
using FluentAssertions;
using Xunit;

namespace EasyBulkCopy.Tests
{
    public class DataTableBuilderTests
    {
        [Fact]
        public void MapToTable_CreatesCorrectColumnNames()
        {
            var row = AutoFaker.Generate<TableWithManyColumns>();
            var bulkMapping = new BulkMapping<TableWithManyColumns>();
            var sut = new DataTableBuilder<TableWithManyColumns>(bulkMapping);

            var result = sut.MapToTable(new[] {row});

            result.Columns.Should().HaveCount(5);
            result.Columns[0].ColumnName.Should().Be(nameof(TableWithManyColumns.Id));
            result.Columns[1].ColumnName.Should().Be(nameof(TableWithManyColumns.Name));
            result.Columns[2].ColumnName.Should().Be(nameof(TableWithManyColumns.ForeignKey));
            result.Columns[3].ColumnName.Should().Be(nameof(TableWithManyColumns.Created));
            result.Columns[4].ColumnName.Should().Be(nameof(TableWithManyColumns.IsActive));
        }

        [Fact]
        public void MapToTable_MapsValuesFromInstances_ToDataTable()
        {
            const int rowCount = 3;
            var rows = AutoFaker.Generate<TableWithManyColumns>(rowCount);
            var bulkMapping = new BulkMapping<TableWithManyColumns>();
            var expectedRows = rows.Select(r => new object[]
            {
                r.Id,
                r.Name,
                r.ForeignKey,
                r.Created,
                r.IsActive
            }).ToArray();

            var sut = new DataTableBuilder<TableWithManyColumns>(bulkMapping);
            var result = sut.MapToTable(rows);

            result.Rows.Should().HaveCount(rowCount);
            result.Rows[0].ItemArray.Should().BeEquivalentTo(expectedRows[0]);
            result.Rows[1].ItemArray.Should().BeEquivalentTo(expectedRows[1]);
            result.Rows[2].ItemArray.Should().BeEquivalentTo(expectedRows[2]);
        }

        [Fact]
        public void MapToTable_MapsTableName_ToDataTable()
        {
            var row = AutoFaker.Generate<TableWithManyColumns>();
            var bulkMapping = new BulkMapping<TableWithManyColumns>();
            var sut = new DataTableBuilder<TableWithManyColumns>(bulkMapping);

            var result = sut.MapToTable(new[] {row});

            result.TableName.Should().Be(nameof(TableWithManyColumns));
        }
    }
}