using System;
using System.Data.SqlClient;
using System.Linq;
using EasyBulkCopy.Tests.TableModels;
using FluentAssertions;
using Xunit;

namespace EasyBulkCopy.Tests
{
    public class BulkMappingTests
    {
        [Fact]
        public void BulkMapping_MapsTableName_FromBulkTableNameAttribute()
        {
            new BulkMapping<TableWithManyColumns>().TableName.Should().Be(nameof(TableWithManyColumns));
        }

        [Fact]
        public void BulkMapping_Maps_StringProperties()
        {
            new BulkMapping<TableWithString>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithString.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_CharProperties()
        {
            new BulkMapping<TableWithChar>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithChar.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_GuidProperties()
        {
            new BulkMapping<TableWithGuid>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithGuid.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_IntProperties()
        {
            new BulkMapping<TableWithInt>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithInt.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_DecimalProperties()
        {
            new BulkMapping<TableWithDecimal>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithDecimal.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_LongProperties()
        {
            new BulkMapping<TableWithLong>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithLong.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_BoolProperties()
        {
            new BulkMapping<TableWithBool>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithBool.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_DateTimeProperties()
        {
            new BulkMapping<TableWithDateTime>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithDateTime.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_DateTimeOffsetProperties()
        {
            new BulkMapping<TableWithDateTimeOffset>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithDateTimeOffset.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_NullableStructProperties()
        {
            new BulkMapping<TableWithNullableBool>().Properties.Should().ContainSingle().Which.Name.Should()
                .Be(nameof(TableWithNullableBool.ColumnName));
        }

        [Fact]
        public void BulkMapping_MapsTablesWithManyProperties_ToProperties()
        {
            var sut = new BulkMapping<TableWithManyColumns>();
            sut.Properties.Select(p => p.Name).Should().BeEquivalentTo(
                new[]
                {
                    nameof(TableWithManyColumns.Id), nameof(TableWithManyColumns.Name),
                    nameof(TableWithManyColumns.ForeignKey), nameof(TableWithManyColumns.Created),
                    nameof(TableWithManyColumns.IsActive)
                });
        }

        [Fact]
        public void BulkMapping_Maps_StringColumns()
        {
            new BulkMapping<TableWithString>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithString.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_CharColumns()
        {
            new BulkMapping<TableWithChar>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithChar.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_GuidColumns()
        {
            new BulkMapping<TableWithGuid>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithGuid.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_IntColumns()
        {
            new BulkMapping<TableWithInt>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithInt.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_DecimalColumns()
        {
            new BulkMapping<TableWithDecimal>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithDecimal.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_LongColumns()
        {
            new BulkMapping<TableWithLong>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithLong.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_BoolColumns()
        {
            new BulkMapping<TableWithBool>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithBool.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_DateTimeColumns()
        {
            new BulkMapping<TableWithDateTime>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithDateTime.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_DateTimeOffsetColumns()
        {
            new BulkMapping<TableWithDateTimeOffset>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithDateTimeOffset.ColumnName));
        }

        [Fact]
        public void BulkMapping_Maps_NullableStructColumns()
        {
            new BulkMapping<TableWithNullableBool>().Columns.Should().ContainSingle().Which.DestinationColumn.Should()
                .Be(nameof(TableWithNullableBool.ColumnName));
        }

        [Fact]
        public void BulkMapping_MapsTablesWithManyProperties_ToColumns()
        {
            var sut = new BulkMapping<TableWithManyColumns>();
            sut.Columns.Select(p => p.DestinationColumn).Should().BeEquivalentTo(
                new[]
                {
                    nameof(TableWithManyColumns.Id), nameof(TableWithManyColumns.Name),
                    nameof(TableWithManyColumns.ForeignKey), nameof(TableWithManyColumns.Created),
                    nameof(TableWithManyColumns.IsActive)
                });
        }

        [Fact]
        public void BulkMapping_ThrowsError_WhenTypeHasNoBulkTableNameAttribute()
        {
            Func<object> act = () => new BulkMapping<DummyBrokenTableWithNoBulkTableAttribute>();

            act.Should().Throw<Exception>()
                .WithMessage(
                    $"No {nameof(BulkTableNameAttribute)} on DummyBrokenTableWithNoBulkTableAttribute.");
        }

        [Fact]
        public void BulkMapping_ExcludesProperties_WithComputedAttribute()
        {
            new BulkMapping<TableWithComputedColumn>().Columns.Should().NotContain(
                c => c.DestinationColumn == nameof(TableWithComputedColumn.IdIsEven));
        }

        [Fact]
        public void BulkMapping_AcceptsSqlBulkOptions_ByDefault()
        {
            new BulkMapping<TableWithInt>().Options.Should().Be(SqlBulkCopyOptions.Default);
        }

        [Fact]
        public void BulkMapping_AcceptsSqlBulkOptions()
        {
            new BulkMapping<DummyTableWithFireTriggersSqlBulkOptions>()
                .Options.Should().Be(SqlBulkCopyOptions.FireTriggers);
        }

        [Fact]
        public void BulkMapping_AcceptsMultipleSqlBulkOptions()
        {
            new BulkMapping<DummyTableWithMultipleSqlBulkOptions>()
                .Options.Should().Be(SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.TableLock);
        }

        [Fact]
        public void BulkMapping_AcceptsSqlBulkCopyOptions()
        {
        }

        private class DummyBrokenTableWithNoBulkTableAttribute
        {
            public int Id { get; set; }
        }


        [BulkTableName("dbo.Test", SqlBulkCopyOptions.FireTriggers)]
        private class DummyTableWithFireTriggersSqlBulkOptions
        {
            public int Id { get; set; }
        }

        [BulkTableName(
            "dbo.Test",
            SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.TableLock)]
        private class DummyTableWithMultipleSqlBulkOptions
        {
            public int Id { get; set; }
        }
    }
}
