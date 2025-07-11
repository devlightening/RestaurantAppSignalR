using SignalRWebUI.Dtos.ContactDtos;
using SignalRWebUI.Dtos.SocialMediaDtos;

namespace SignalRWebUI.Models.Footer
{
    public class FooterandContactViewModel
    {
        public ResultContactDto Footer { get; set; }
        public ResultContactDto Contact { get; set; }
        public List<ResultSocialMediaDto> SocialMedia { get; set; }
    }
}