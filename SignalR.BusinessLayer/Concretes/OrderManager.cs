using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Concretes
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;

        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public async Task TAddAsync(Order entity)
        {
            await _orderDal.AddAsync(entity);
            await _orderDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task TDeleteAsync(Order entity)
        {
            await _orderDal.DeleteAsync(entity);
            await _orderDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task<Order> TGetByIdAsync(int id)
        {
            return await _orderDal.GetByIdAsync(id);
        }

        public async Task<List<Order>> TGetListAllAsync()
        {
            return await _orderDal.GetListAllAsync();
        }

        public async Task<List<Order>> TGetListWithOrderDetails()
        {
           
            return await _orderDal.GetListWithOrderDetails();
        }

        public async Task<decimal> TLastOrderPrice()
        {
         
            return await _orderDal.LastOrderPrice();
        }

        public async Task TSaveChangesAsync()
        {
            await _orderDal.SaveChangesAsync();
        }

        public async Task<decimal> TTodayTotalPrice()
        {
          
            return await _orderDal.TodayTotalPrice();
        }

        public async Task<int> TTotalOrderNumber()
        {
         
            return await _orderDal.TotalOrderNumber();
        }

        public async Task TUpdateAsync(Order entity)
        {
            await _orderDal.UpdateAsync(entity);
            await _orderDal.SaveChangesAsync();
        }

   
        public async Task<int> TActiveOrderNumber()
        {
            return await _orderDal.ActiveOrderNumber();
        }
    }
}
