using AutoMapper;
using Domain.Models;
using Shared;

namespace Services.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            // Basket Items Mapping
            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ReverseMap()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            // ✅ Basket Mapping
            CreateMap<CustomerBasket, BasketDto>()
                .ReverseMap();
        }
    }
}
