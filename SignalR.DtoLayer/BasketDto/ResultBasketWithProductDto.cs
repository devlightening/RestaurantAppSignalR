using SignalR.DtoLayer.ProductDto;
using SignalR.DtoLayer.RestaurantTableDto;

namespace SignalR.DtoLayer.BasketDto
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
        public string ProductName { get; set; }
    }
}
