using AutoMapper;
using Products.Application.Products.Responses;
using Products.Domain.Entities;

namespace Products.Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Value))
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock.Value))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.Stock.Value > 0));
    }
}