using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.EntityLayer.Entities
{
    public class Basket
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int RestaurantTableId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
        public Product Product { get; set; }

        public RestaurantTable RestaurantTable { get; set; }
    }
}
