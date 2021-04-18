using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBulkCopy
{
    public interface IBulkInserter
    {
        Task Insert<T>(string connectionString, IList<T> records) where T : class;
    }
}