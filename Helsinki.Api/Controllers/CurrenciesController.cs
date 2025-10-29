using Helsinki.Application.Interfaces.Providers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Helsinki.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly IRateProviderFactory _factory;
        public CurrenciesController(IRateProviderFactory factory) => _factory = factory;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<string>>> Get(CancellationToken ct)
            => (await _factory.Create().GetCurrenciesAsync(ct)).ToList();
    }
}
