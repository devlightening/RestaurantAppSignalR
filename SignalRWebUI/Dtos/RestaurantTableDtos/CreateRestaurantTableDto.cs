using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRWebUI.Dtos.RestaurantTableDtos
{
    public class CreateRestaurantTableDto
    {
        public string TableName { get; set; }
        public bool Status { get; set; }
    }
}
