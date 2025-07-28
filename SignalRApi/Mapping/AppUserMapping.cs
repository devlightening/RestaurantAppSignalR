using AutoMapper;
using SignalR.DtoLayer.AppUserDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
    public class AppUserMapping : Profile
    {
        public AppUserMapping()
        {
            // CreateAppUserDto => AppUser (create işlemi için)
            CreateMap<CreateAppUserDto, AppUser>().ReverseMap();
            // AppUser => ResultAppUserDto (response için)
            CreateMap<AppUser, ResultAppUserDto>().ReverseMap();
        }
    }
}
