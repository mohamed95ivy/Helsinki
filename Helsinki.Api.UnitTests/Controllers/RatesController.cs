namespace Helsinki.Api.UnitTests.Controllers
{
    public class RatesController
    {
        // 1) Get_ReturnsOk_WithMap_ForBase
        //    - Provider returns { "EUR":1m, "USD":1.1m } for base="EUR".
        //    - Assert 200 OK and dictionary contains expected keys/values.

        // 2) Get_Forwards_BaseCurrency_Unchanged
        //    - Call with baseCurrency = "eUr".
        //    - Verify IExchangeRateProvider.GetRatesAsync("eUr", token) received exact string.

        // 3) Get_CallsFactoryCreate_ExactlyOnce
        //    - Verify IRateProviderFactory.Create() invoked once per request.

        // 4) Get_PropagatesCancellationToken
        //    - Pass a CTS.Token.
        //    - Verify provider.GetRatesAsync(base, sameToken) called with that token.

    }
}


/// Etc.. For other projects