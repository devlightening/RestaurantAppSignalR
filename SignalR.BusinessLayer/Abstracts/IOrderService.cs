using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IOrderService : IGenericService<Order>
    {
        public int TTotalOrderNumber();
        public int TActiveOrderNumber();
        public decimal TLastOrderPrice();
        public decimal TTodayTotalPrice();
        public List<Order> TGetListWithOrderDetails();
    }
}
