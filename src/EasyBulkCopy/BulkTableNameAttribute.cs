using System;
using System.Data.SqlClient;

namespace EasyBulkCopy
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BulkTableNameAttribute : Attribute
    {
        public BulkTableNameAttribute(string tableName)
        {
            TableName = tableName;
        }

        public BulkTableNameAttribute(string tableName, SqlBulkCopyOptions sqlBulkCopyOptions) : this(tableName)
        {
            Options = sqlBulkCopyOptions;
        }

        public string TableName { get; }
        public SqlBulkCopyOptions Options { get; }
    }
}
