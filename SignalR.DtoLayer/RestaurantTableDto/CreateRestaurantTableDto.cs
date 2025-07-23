using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DtoLayer.RestaurantTableDto 
{
    public class CreateRestaurantTableDto
    {
        public int TableNo { get; set; }
        public bool Status { get; set; }
        public TableLocation Location { get; set; }

    }
}
