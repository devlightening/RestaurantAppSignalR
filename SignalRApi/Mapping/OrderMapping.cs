using AutoMapper;
using SignalR.DtoLayer.OrderDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping() {

            CreateMap<Order, ResultOrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderDto>().ReverseMap();
            CreateMap<Order, GetOrderDto>().ReverseMap();

        }

    }
}
