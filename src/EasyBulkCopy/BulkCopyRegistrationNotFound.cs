using System;

namespace EasyBulkCopy
{
    public class BulkCopyRegistrationNotFound : Exception
    {
        public BulkCopyRegistrationNotFound(Type type) : base($"No bulk copy registration found for {type.Name}.")
        {
        }
    }
}