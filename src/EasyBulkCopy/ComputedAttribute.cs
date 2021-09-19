using System;

namespace EasyBulkCopy
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ComputedAttribute : Attribute
    {
        public ComputedAttribute()
        {
        }
    }
}
