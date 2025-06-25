using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IOrderDal : IGenericDal<Order>
    {
        public int TotalOrderNumber();
        public int ActiveOrderNumber();
        public decimal LastOrderPrice();
        public decimal TodayTotalPrice();
        public List<Order> GetListWithOrderDetails();
    }
}
