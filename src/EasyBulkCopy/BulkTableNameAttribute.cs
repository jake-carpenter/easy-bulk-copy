using System;

namespace EasyBulkCopy
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BulkTableNameAttribute : Attribute
    {
        public BulkTableNameAttribute(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; }
    }
}