using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBulkCopy
{
    internal class BulkInserter : IBulkInserter
    {
        private readonly Dictionary<Type,IDataTableBuilder> _dataTableBuilders;

        public BulkInserter(IEnumerable<IDataTableBuilder> dataTableBuilders)
        {
            _dataTableBuilders = dataTableBuilders.ToDictionary(x => x.Type);
        }

        public async Task Insert<T>(string connectionString, IList<T> records) where T : class
        {
            var dataTableBuilder = GetTableBuilder(typeof(T));
            var table = dataTableBuilder.MapToTable<T>(records);

            using (var bulkCopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.TableLock))
            {
                bulkCopy.BatchSize = records.Count;
                bulkCopy.DestinationTableName = table.TableName;

                AddColumnsToBulkCopy<T>(dataTableBuilder, bulkCopy);
                await bulkCopy.WriteToServerAsync(table);
            }
        }

        private static void AddColumnsToBulkCopy<T>(IDataTableBuilder dataTableBuilder, SqlBulkCopy bulkCopy)
        {
            foreach (var column in dataTableBuilder.Columns)
            {
                bulkCopy.ColumnMappings.Add(column);
            }
        }

        private IDataTableBuilder GetTableBuilder(Type type)
        {
            if (!_dataTableBuilders.TryGetValue(type, out var dataTableBuilder))
                throw new BulkCopyRegistrationNotFound(type);

            return dataTableBuilder;
        }
    }
}