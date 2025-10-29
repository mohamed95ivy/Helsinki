namespace Helsinki.Api.Dtos
{
    public class PaginationResult
    {
        public int Count { get; set; }
        public IList<ConversionResponseDto> Items { get; set; }
    }
}
