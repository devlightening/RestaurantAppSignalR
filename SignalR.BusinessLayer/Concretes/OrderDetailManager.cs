using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Concretes
{
    public class OrderDetailManager : IOrderDetailService
    {
        private readonly IOrderDetailDal _orderDetailDal;

        public OrderDetailManager(IOrderDetailDal orderDetailDal)
        {
            _orderDetailDal = orderDetailDal;
        }

        public async Task TAddAsync(OrderDetail entity)
        {
            await _orderDetailDal.AddAsync(entity);
            await _orderDetailDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task TDeleteAsync(OrderDetail entity)
        {
            await _orderDetailDal.DeleteAsync(entity);
            await _orderDetailDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task<OrderDetail> TGetByIdAsync(int id)
        {
            return await _orderDetailDal.GetByIdAsync(id);
        }

        public async Task<List<OrderDetail>> TGetListAllAsync()
        {
            return await _orderDetailDal.GetListAllAsync();
        }

        public async Task TSaveChangesAsync()
        {
            await _orderDetailDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }

        public async Task TUpdateAsync(OrderDetail entity)
        {
            await _orderDetailDal.UpdateAsync(entity);
            await _orderDetailDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }
    }
}
