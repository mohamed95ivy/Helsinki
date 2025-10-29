using System.ComponentModel.DataAnnotations;

namespace Helsinki.Api.Dtos
{
    public class ConversionRequestDto
    {
        [Required, StringLength(3, MinimumLength = 3)]
        public string FromCurrency { get; init; } = default!;

        [Required, StringLength(3, MinimumLength = 3)]
        public string ToCurrency { get; init; } = default!;

        [Range(0, double.MaxValue)]
        public decimal Amount { get; init; }

        public string? UserId { get; init; }

    }
}
