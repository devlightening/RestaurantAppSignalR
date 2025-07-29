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
        Task<int> TTotalOrderNumber();
        Task<int> TActiveOrderNumber();
        Task<decimal> TLastOrderPrice();
        Task<decimal> TTodayTotalPrice();
        Task<List<Order>> TGetListWithOrderDetails();
    }
}
