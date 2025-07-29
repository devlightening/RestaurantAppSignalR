using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfOrderDal : GenericRepository<Order>, IOrderDal
    {
        private readonly SignalRContext _context;

        public EfOrderDal(SignalRContext context) : base(context)
        {
            _context = context; // Dependency Injection ile gelen context'i kullanıyoruz
        }

        // Aktif sipariş sayısını asenkron olarak döndürür
        public async Task<int> ActiveOrderNumber()
        {
            // Eski mantığa göre "Müşteri Masada" durumundaki siparişleri sayar
            return await _context.Orders.Where(x => x.Description == "Müşteri Masada").CountAsync();
        }

        // Sipariş detayları ile birlikte sipariş listesini asenkron olarak döndürür
        public async Task<List<Order>> GetListWithOrderDetails()
        {
            // Siparişleri OrderDetails navigation property'si ile birlikte yükler
            return await _context.Orders.Include(x => x.OrderDetails).ToListAsync();
        }

        // Son siparişin fiyatını asenkron olarak döndürür
        public async Task<decimal> LastOrderPrice()
        {
            // En son siparişin (OrderId'ye göre azalan) toplam fiyatını alır
            // FirstOrDefaultAsync, Take(1) yerine daha uygun bir asenkron alternatiftir
            return await _context.Orders
                                 .OrderByDescending(x => x.OrderId)
                                 .Select(y => y.TotalOrderPrice)
                                 .FirstOrDefaultAsync();
        }

        // Bugünün toplam sipariş fiyatını asenkron olarak döndürür
        public async Task<decimal> TodayTotalPrice()
        {
            // Sadece bugüne ait ve "Hesap Kapatıldı" durumundaki siparişlerin toplam fiyatını hesaplar
            DateTime today = DateTime.Now.Date; // Sadece tarih kısmını alır
            return await _context.Orders
                                 .Where(x => x.OrderDate.Date == today && x.Description == "Hesap Kapatıldı")
                                 .SumAsync(y => y.TotalOrderPrice);
        }

        // Toplam sipariş sayısını asenkron olarak döndürür
        public async Task<int> TotalOrderNumber()
        {
            // Tüm siparişlerin sayısını asenkron olarak döner
            return await _context.Orders.CountAsync();
        }
    }
}
