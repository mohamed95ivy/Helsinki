namespace Helsinki.Api.UnitTests.Controllers
{
    public class CurrenciesControllerTests
    {
        // 1) Get_ReturnsOk_WithList
        //    - Arrange provider to return ["EUR","GBP","USD"].
        //    - Assert OkObjectResult with same list contents.

        // 2) Get_ReturnsOk_WithEmptyList
        //    - Provider returns empty array.
        //    - Assert 200 OK and empty result.

        // 3) Get_CallsFactoryCreate_ExactlyOnce
        //    - Verify IRateProviderFactory.Create() invoked once.

        // 4) Get_PropagatesCancellationToken
        //    - Pass a CancellationTokenSource.Token to action.
        //    - Verify provider.GetCurrenciesAsync(token) received the same token.
    }
}
