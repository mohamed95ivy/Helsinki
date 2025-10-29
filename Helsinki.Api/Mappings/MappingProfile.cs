using AutoMapper;
using Helsinki.Api.Dtos;
using Helsinki.Domain.Entities;

namespace Helsinki.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ConversionHistory, ConversionResponseDto>();
        }
    }
}
