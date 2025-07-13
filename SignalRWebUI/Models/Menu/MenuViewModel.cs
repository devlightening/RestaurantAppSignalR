using SignalRWebUI.Dtos.ProductDtos;

namespace SignalRWebUI.Models.Menu
{
    public class MenuViewModel
    {
        public List<ResultProductDto> Products { get; set; }
        public List<string> Categories { get; set; }
    }

}
