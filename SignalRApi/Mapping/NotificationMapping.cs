using AutoMapper;
using SignalR.DtoLayer.NotificationDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
    public class NotificationMapping : Profile
    {
        public NotificationMapping()
        {
            CreateMap<Notification, ResultNotificationDto>().ReverseMap();
            CreateMap<Notification, GetNotificationDto>().ReverseMap();
            CreateMap<Notification, UpdateNotificationDto>().ReverseMap();
            CreateMap<Notification, CreateNotificationDto>().ReverseMap();
        }

    }
}
