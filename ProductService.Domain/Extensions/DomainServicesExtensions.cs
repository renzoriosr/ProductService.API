using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Common.Configuration;

namespace ProductService.Domain.Extensions
{
    public static class DomainServicesExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));
            //services.AddSingleton(new CacheConfiguration());

            return services;
        }
    }
}
