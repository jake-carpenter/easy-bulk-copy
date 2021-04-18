using System;
using System.Threading.Tasks;
using AutoBogus;
using EasyBulkCopy.Tests.TableModels;
using FluentAssertions;
using Xunit;

namespace EasyBulkCopy.Tests
{
    public class BulkInserterTests : IClassFixture<DatabaseEnvironmentFixture>, IAsyncLifetime
    {
        private readonly DatabaseEnvironmentFixture _fixture;

        public BulkInserterTests(DatabaseEnvironmentFixture fixture)
        {
            _fixture = fixture;
        }

        public Task InitializeAsync() => Task.CompletedTask;
        public Task DisposeAsync() => _fixture.ClearTables();

        [Fact]
        public async Task BulkInserter_InsertsMultipleRecords()
        {
            var rows = AutoFaker.Generate<TableWithManyColumns>(10);
            var bulkMapping = new BulkMapping<TableWithManyColumns>();
            var dataTableBuilder = new DataTableBuilder<TableWithManyColumns>(bulkMapping);
            var sut = new BulkInserter(new[] {dataTableBuilder});

            await sut.Insert(_fixture.ConnectionString, rows);

            _fixture.GetAllRecordsInTable<TableWithManyColumns>(nameof(TableWithManyColumns)).Should()
                .BeEquivalentTo(rows);
        }

        [Fact]
        public async Task BulkInserter_SelectsCorrectFromManyTableRegistrations()
        {
            var fullMapping = new BulkMapping<TableWithManyColumns>();
            var boolMapping = new BulkMapping<TableWithBool>();
            var guidMapping = new BulkMapping<TableWithGuid>();
            var fullTableBuilder = new DataTableBuilder<TableWithManyColumns>(fullMapping);
            var boolTableBuilder = new DataTableBuilder<TableWithBool>(boolMapping);
            var guidTableBuilder = new DataTableBuilder<TableWithGuid>(guidMapping);
            var rows = AutoFaker.Generate<TableWithManyColumns>(10);
            var sut = new BulkInserter(new IDataTableBuilder[]
            {
                boolTableBuilder, guidTableBuilder, fullTableBuilder
            });

            await sut.Insert(_fixture.ConnectionString, rows);

            _fixture.GetAllRecordsInTable<TableWithManyColumns>(nameof(TableWithManyColumns)).Should()
                .BeEquivalentTo(rows);
        }

        [Fact]
        public async Task BulkInserter_Throws_IfStrategyForInsertingIsNotFound()
        {
            var row = AutoFaker.Generate<TableWithManyColumns>();
            var sut = new BulkInserter(Array.Empty<IDataTableBuilder>());

            Func<Task> act = () => sut.Insert(_fixture.ConnectionString, new[] {row});

            await act.Should().ThrowAsync<BulkCopyRegistrationNotFound>();
        }
    }
}