namespace Helsinki.Api.Dtos
{
    public class ConversionResponseDto
    {
        public Guid ConversionId { get; init; }
        public string FromCurrency { get; init; } = default!;
        public string ToCurrency { get; init; } = default!;
        public decimal FromAmount { get; init; }
        public decimal ToAmount { get; init; }
        public decimal ExchangeRate { get; init; }
        public DateTimeOffset ConversionDate { get; init; }

    }
}
