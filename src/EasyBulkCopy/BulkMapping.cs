using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EasyBulkCopy.Tests")]
namespace EasyBulkCopy
{
    internal class BulkMapping<T> where T : class
    {
        public BulkMapping()
        {
            TableName = GetTableName();
            Properties = GetProperties().ToArray();
            Columns = BuildColumnList(Properties).ToList();
        }

        public string TableName { get; }
        public IReadOnlyList<PropertyDescriptor> Properties { get; }
        public IReadOnlyList<SqlBulkCopyColumnMapping> Columns { get; }

        private static IEnumerable<PropertyDescriptor> GetProperties()
        {
            return TypeDescriptor
                .GetProperties(typeof(T))
                .Cast<PropertyDescriptor>()
                .Where(property => property?.PropertyType?.Namespace?.Equals("System") ?? false)
                .Where(property => property?.Attributes.Contains(new ComputedAttribute()) == false);
        }

        private static IEnumerable<SqlBulkCopyColumnMapping> BuildColumnList(IEnumerable<PropertyDescriptor> properties)
        {
            foreach (var descriptor in properties)
            {
                var propertyInfo = descriptor.ComponentType.GetProperty(descriptor.Name);
                yield return new SqlBulkCopyColumnMapping(descriptor.Name, propertyInfo?.Name);
            }
        }

        private static string GetTableName()
        {
            var attribute = typeof(T)
                                .GetCustomAttributes(typeof(BulkTableNameAttribute), false)
                                .FirstOrDefault() as BulkTableNameAttribute
                            ?? throw new Exception($"No {nameof(BulkTableNameAttribute)} on {typeof(T).Name}.");

            return attribute?.TableName;
        }
    }
}