using AutoMapper;
using SignalR.DtoLayer.AppUserDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
    public class AppUserMapping : Profile
    {
        public AppUserMapping()
        {
            CreateMap<AppUser, CreateAppUserDto>().ReverseMap();
            CreateMap<AppUser, ResultAppUserDto>().ReverseMap();
            CreateMap<AppUser, UpdateAppUserDto>().ReverseMap();

        }
    }
}
