using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyBulkCopy.TestApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConfig = new DatabaseConfig();
            var section = Configuration.GetSection(nameof(DatabaseConfig));
            section.Bind(databaseConfig);

            var dbEnvironment = new DatabaseEnvironment(databaseConfig);
            services.AddSingleton(_ => dbEnvironment);

            services.UseEasyBulkCopy();
            services.RegisterBulkInsertTypesInAssembly(Assembly.GetExecutingAssembly());
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
