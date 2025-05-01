using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models.Identity;
using Domain.Models.OrderModels;
using Shared.OrdersModels;
using Address = Domain.Models.OrderModels.Address;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                      .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                      .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                      .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl));


            CreateMap<Order, OrderResultDto>()
                     .ForMember(d => d.OrderPaymentStatus, o => o.MapFrom(d => d.OrderPaymentStatus.ToString()))
                     .ForMember(d => d.DeliveryMethod, o => o.MapFrom(d => d.DeliveryMethod.ShortName))
                     .ForMember(d => d.Total, o => o.MapFrom(d => d.SubTotal + d.DeliveryMethod.Cost));

            CreateMap<DeliveryMethod, DeliveryMethodDto>();

        }
    }
}
