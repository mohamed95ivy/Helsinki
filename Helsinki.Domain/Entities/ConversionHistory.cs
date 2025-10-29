namespace Helsinki.Domain.Entities
{
    public class ConversionHistory
    {
        public Guid ConversionId { get; set; } = Guid.NewGuid();
        public string FromCurrency { get; set; } = default!; // ISO 4217
        public string ToCurrency { get; set; } = default!; // ISO 4217
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime ConversionDate { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = "candidate"; // hardcoded for assessment

    }
}
