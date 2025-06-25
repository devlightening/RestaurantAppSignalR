using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRWebUI.Dtos.OrderDtos
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        public string TableNumber { get; set; }
        public string Description { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalOrderPrice { get; set; }
    }
}
