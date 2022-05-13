using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Interfaces.Infrastructure;
using ProductService.Domain.Interfaces.Repository;
using ProductService.Infrastructure.Caching;
using ProductService.Infrastructure.Configurations;
using ProductService.Infrastructure.Connector;
using ProductService.Infrastructure.EFCoreInMemory.Repositories;

namespace ProductService.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<ApiContext>(opt =>
            {
                opt.UseInMemoryDatabase("Test");
            });

            /*services.AddScoped<IProductRepository, ProductRepository>();*/
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: use app settings to drive cache service implementation (noop, memory, redis)
            services.AddMemoryCache();
            services.AddScoped<ICacheService, CacheService>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddHttpClient<IExternalServiceConnector, ExternalServiceConnector>(client =>
            {
                client.BaseAddress = new Uri(configuration["ExternalServiceBaseAddress"]);
            });

            return services;
        }
    }
}
