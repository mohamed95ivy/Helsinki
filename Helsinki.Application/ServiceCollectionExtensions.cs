using Helsinki.Application.Interfaces.Services;
using Helsinki.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Helsinki.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IConversionService, ConversionService>();

            return services;
        }
    }
}
