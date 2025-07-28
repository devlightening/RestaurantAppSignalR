using AutoMapper;
using SignalR.DtoLayer.MessageDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
    public class MessageMapping : Profile
    {
        public MessageMapping()
        {
          CreateMap<Message, ResultMessageDto>()
                 .ForMember(dest => dest.SenderFullName, opt => opt.MapFrom(src =>
                     (src.SenderUser != null ? src.SenderUser.Name + " " + src.SenderUser.Surname : "Bilinmiyor")))
                 .ForMember(dest => dest.ReceiverFullName, opt => opt.MapFrom(src =>
                     (src.ReceiverUser != null ? src.ReceiverUser.Name + " " + src.ReceiverUser.Surname : "Bilinmiyor")));


            CreateMap<CreateMessageDto, Message>().ReverseMap();
            CreateMap<Message, GetMessageByIdDto>().ReverseMap();


        }

    }
}
