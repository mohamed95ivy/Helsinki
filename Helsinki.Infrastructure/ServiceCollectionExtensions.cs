using Helsinki.Application.Interfaces.Providers;
using Helsinki.Application.Interfaces.Repositories;
using Helsinki.Infrastructure.Options;
using Helsinki.Infrastructure.Providers;
using Helsinki.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Helsinki.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(opts =>
            {
                var cs = config.GetConnectionString("Default") ?? "Data Source=app.db";
                opts.UseSqlite(cs, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            services.AddMemoryCache();

            services.AddHttpClient<IExchangeRateProvider, OpenExchangeRatesProvider>(c =>
            {
                c.BaseAddress = new Uri("https://openexchangerates.org/");
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                c.Timeout = TimeSpan.FromSeconds(10);
            });

            services.Configure<OpenExchangeRatesOptions>(config.GetSection(OpenExchangeRatesOptions.SectionName));
            services.AddScoped<IRateProviderFactory, RateProviderFactory>();
            services.AddScoped<IConversionRepository, ConversionRepository>();
            // Apply migrations at startup
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();

            return services;
        }
    }
}

