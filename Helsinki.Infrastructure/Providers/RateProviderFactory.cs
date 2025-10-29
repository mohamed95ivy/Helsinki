using Helsinki.Application.Interfaces.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Helsinki.Infrastructure.Providers
{
    public class RateProviderFactory : IRateProviderFactory
    {
        private readonly IServiceProvider _sp;
        public RateProviderFactory(IServiceProvider sp) => _sp = sp;

        public IExchangeRateProvider Create() => _sp.GetRequiredService<IExchangeRateProvider>();
    }
}
