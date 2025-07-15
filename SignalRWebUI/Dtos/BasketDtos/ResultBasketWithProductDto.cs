
using SignalRWebUI.Dtos.ProductDtos;
using SignalRWebUI.Dtos.RestaurantTableDtos;

namespace SignalRWebUI.Dtos.BasketDtos
{
    public class ResultBasketWithProductDto
    {
        public int BasketId { get; set; }
        public ResultProductDto Product { get; set; }
        public ResultRestaurantTableDto RestaurantTable { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
    }
}
