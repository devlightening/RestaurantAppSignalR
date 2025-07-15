using AutoMapper;
using SignalR.DtoLayer.BasketDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
    public class BasketMapping : Profile
    {
        public BasketMapping()
        {
            CreateMap<Basket, ResultBasketDto>().ReverseMap();
            CreateMap<Basket, CreateBasketDto>().ReverseMap();
            CreateMap<Basket, UpdateBasketDto>().ReverseMap();
            CreateMap<Basket, GetBasketDto>().ReverseMap();
            CreateMap<Basket, ResultBasketWithProductDto>()
                    .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                    .ForMember(dest => dest.RestaurantTable, opt => opt.MapFrom(src => src.RestaurantTable));



        }

    }
}
