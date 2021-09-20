using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace EasyBulkCopy
{
    public interface IDataTableBuilder
    {
        Type Type { get; }
        IReadOnlyList<SqlBulkCopyColumnMapping> Columns { get; }
        IReadOnlyList<PropertyDescriptor> Properties { get; }
        SqlBulkCopyOptions Options { get; }
        DataTable MapToTable<T>(IEnumerable<T> records) where T : class;
    }
}