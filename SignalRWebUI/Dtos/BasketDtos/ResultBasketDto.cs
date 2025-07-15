using SignalRWebUI.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRWebUI.Dtos.BasketDtos
{
    public class ResultBasketDto
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int RestaurantTableId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
        public ResultProductDto Product { get; set; }
        public string ProductName { get; set; }


    }
}
