using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IOrderDal : IGenericDal<Order>
    {
         Task<int> TotalOrderNumber();
         Task<int> ActiveOrderNumber();
         Task<decimal> LastOrderPrice();
         Task<decimal> TodayTotalPrice();
         Task<List<Order>> GetListWithOrderDetails();
    }
}
