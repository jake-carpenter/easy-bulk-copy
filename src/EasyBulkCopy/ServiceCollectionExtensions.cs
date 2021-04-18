using Microsoft.Extensions.DependencyInjection;

namespace EasyBulkCopy
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterBulkInsertType<T>(this IServiceCollection services) where T : class
        {
            var dataTableBuilder = new DataTableBuilder<T>(new BulkMapping<T>());
            services.AddSingleton<IDataTableBuilder>(_ => dataTableBuilder);
        }

        public static void UseEasyBulkCopy(this IServiceCollection services)
        {
            services.AddSingleton<IBulkInserter, BulkInserter>();
        }
    }
}