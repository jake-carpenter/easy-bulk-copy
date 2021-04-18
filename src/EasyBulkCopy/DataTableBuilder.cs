using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EasyBulkCopy.Tests")]
namespace EasyBulkCopy
{
    internal class DataTableBuilder<TTable> : IDataTableBuilder where TTable : class
    {
        private readonly BulkMapping<TTable> _bulkMapping;
        private readonly DataTable _tableTemplate;

        public DataTableBuilder(BulkMapping<TTable> bulkMapping)
        {
            _bulkMapping = bulkMapping;
            _tableTemplate = CreateTableTemplate(bulkMapping.Properties, bulkMapping.TableName);
        }

        public IReadOnlyList<PropertyDescriptor> Properties => _bulkMapping.Properties;
        public IReadOnlyList<SqlBulkCopyColumnMapping> Columns => _bulkMapping.Columns;
        public bool AppliesTo(Type type) => type == typeof(TTable);

        public DataTable MapToTable<T>(IEnumerable<T> records) where T : class
        {
            var table = _tableTemplate.Clone();
            var values = new object[_bulkMapping.Properties.Count];

            foreach (var record in records)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = _bulkMapping.Properties[i].GetValue(record);
                }

                table.Rows.Add(values);
            }

            return table;
        }

        private static DataTable CreateTableTemplate(
            IEnumerable<PropertyDescriptor> propertyDescriptors,
            string tableName)
        {
            var table = new DataTable {TableName = tableName};

            foreach (var propertyDescriptor in propertyDescriptors)
            {
                var type = Nullable.GetUnderlyingType(propertyDescriptor.PropertyType)
                           ?? propertyDescriptor.PropertyType;

                table.Columns.Add(propertyDescriptor.Name, type);
            }

            return table;
        }
    }
}