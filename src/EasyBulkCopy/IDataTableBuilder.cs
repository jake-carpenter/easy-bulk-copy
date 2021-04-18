using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace EasyBulkCopy
{
    public interface IDataTableBuilder
    {
        IReadOnlyList<SqlBulkCopyColumnMapping> Columns { get; }
        IReadOnlyList<PropertyDescriptor> Properties { get; }
        bool AppliesTo(Type type);
        DataTable MapToTable<T>(IEnumerable<T> records) where T : class;
    }
}