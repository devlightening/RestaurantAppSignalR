using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IOrderDal : IGenericDal<Order>
    {
        public int TotalOrderNumber();
        public int ActiveOrderNumber();
        public decimal LastOrderPrice();
        public decimal TodayTotalPrice();
    }
}
