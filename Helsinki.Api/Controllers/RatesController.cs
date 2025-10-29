using Helsinki.Application.Interfaces.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Helsinki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly IRateProviderFactory _factory;
        public RatesController(IRateProviderFactory factory) => _factory = factory;

        [HttpGet("{baseCurrency}")]
        public async Task<ActionResult<IDictionary<string, decimal>>> Get(string baseCurrency, CancellationToken ct)
            => Ok(await _factory.Create().GetRatesAsync(baseCurrency, ct));

    }
}
