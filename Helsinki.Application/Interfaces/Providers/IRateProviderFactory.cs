namespace Helsinki.Application.Interfaces.Providers
{
    public interface IRateProviderFactory
    {
        IExchangeRateProvider Create();

    }
}
