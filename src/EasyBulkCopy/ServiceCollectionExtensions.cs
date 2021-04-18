using System;
using System.Reflection;
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

        public static void RegisterBulkInsertTypesInAssembly(this IServiceCollection services, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttribute<BulkTableNameAttribute>();
                if (attribute == null)
                    continue;

                var bulkMappingType = typeof(BulkMapping<>);
                var bulkMappingGeneric = bulkMappingType.MakeGenericType(type);
                var bulkMapping = Activator.CreateInstance(bulkMappingGeneric);

                var builderType = typeof(DataTableBuilder<>);
                var builderGeneric = builderType.MakeGenericType(type);
                var builder = Activator.CreateInstance(builderGeneric, bulkMapping) as IDataTableBuilder;

                services.AddSingleton(_ => builder);
            }
        }

        public static void UseEasyBulkCopy(this IServiceCollection services)
        {
            services.AddSingleton<IBulkInserter, BulkInserter>();
        }
    }
}