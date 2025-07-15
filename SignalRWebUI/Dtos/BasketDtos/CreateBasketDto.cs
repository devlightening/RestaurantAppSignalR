using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRWebUI.Dtos.BasketDtos
{
    public class CreateBasketDto
    {
        public int ProductId { get; set; }
        public int RestaurantTableId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
    }
}
