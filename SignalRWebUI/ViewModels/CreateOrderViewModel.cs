using SignalRWebUI.Dtos.OrderDtos;
using SignalRWebUI.Dtos.ProductDtos; // ResultProductDto burada olmalı

namespace SignalRWebUI.ViewModels
{
    public class CreateOrderViewModel
    {
        public CreateOrderDto Order { get; set; }
        public List<ResultProductDto> Products { get; set; }

        // Kullanıcının seçtiği ürün ID'leri burada tutulur (post edilecek)
        public List<int> SelectedProductIds { get; set; } = new List<int>();
    }
}
