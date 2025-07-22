using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.EntityLayer.Entities
{
    public class RestaurantTable
    {
        public int RestaurantTableId { get; set; }
        public string TableName { get; set; }
        public bool Status { get; set; }
        public List<Basket> Baskets { get; set; }
        public ICollection<Order> Order { get; set; }
        public TableLocation Location { get; set; }
    }

    public enum TableLocation
    {
        Salon = 0,
        Balkon = 1,
        UstKat = 2
    }

}
