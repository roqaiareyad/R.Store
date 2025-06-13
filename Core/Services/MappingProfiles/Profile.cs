using AutoMapper;
using Shared.OrdersModels;
using Domain.Models.Identity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddressDto, Address>();
        CreateMap<Address, AddressDto>();
    }
}
