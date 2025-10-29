using AutoMapper;
using Helsinki.Api.Dtos;
using Helsinki.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Helsinki.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _service;
        private readonly IMapper _mapper;

        public ConversionController(IConversionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // POST /api/conversion
        [HttpPost]
        [ProducesResponseType(typeof(ConversionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // validation, bad currency, negative amount
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)] // external rates down

        public async Task<ActionResult<ConversionResponseDto>> Convert([FromBody] ConversionRequestDto req, CancellationToken ct)
        {
            var userId = string.IsNullOrWhiteSpace(req.UserId) ? "candidate" : req.UserId!;
            var record = await _service.ConvertAsync(userId, req.FromCurrency, req.ToCurrency, req.Amount, ct);
            var dto = _mapper.Map<ConversionResponseDto>(record);
            return dto;
        }

        // GET /api/conversion/history?skip=0&take=50&sort=desc&userId=candidate
        [HttpGet("history")]
        [ProducesResponseType(typeof(IReadOnlyList<ConversionResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationResult>> History(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            [FromQuery] string sort = "desc",
            [FromQuery] string userId = "candidate",
            CancellationToken ct = default)
        {
            take = Math.Clamp(take, 1, 200);
            var newestFirst = !string.Equals(sort, "asc", StringComparison.OrdinalIgnoreCase);
            var items = await _service.GetHistoryAsync(userId, skip, take, newestFirst, ct);
            var total = await _service.GetTotalConversions(userId);
           
            var result = new PaginationResult
            {
                Count = total,
                Items = _mapper.Map<IList<ConversionResponseDto>>(items)
            };
            return result;
        }
    }
}
