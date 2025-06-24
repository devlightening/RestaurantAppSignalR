using SignalR.DtoLayer.RestaurantTableDto;
using SignalR.EntityLayer.Entities;
using AutoMapper;

namespace SignalRApi.Mapping
{
    public class RestaurantTableMapping : Profile
    {
        public RestaurantTableMapping()
        {
            CreateMap<RestaurantTable, ResultRestaurantTableDto>().ReverseMap();
            CreateMap<RestaurantTable, CreateRestaurantTableDto>().ReverseMap();
            CreateMap<RestaurantTable, UpdateRestaurantTableDto>().ReverseMap();
            CreateMap<RestaurantTable, GetRestaurantTableDto>().ReverseMap();
        }
    }
}
