using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfOrderDal : GenericRepository<Order>, IOrderDal
    {
        public EfOrderDal(SignalRContext context) : base(context)
        {
        }

        public int ActiveOrderNumber()
        {
            using var context = new SignalRContext();
            return context.Orders.Where(x=>x.Description== "Müşteri Masada").Count();
        }

        public List<Order> GetListWithOrderDetails()
        {
            using var context = new SignalRContext();
            return context.Orders.Include(x => x.OrderDetails).ToList();

        }

        public decimal LastOrderPrice()
        {
            using var context = new SignalRContext();
            return context.Orders.OrderByDescending(x => x.OrderId).Take(1).Select(y => y.TotalOrderPrice).FirstOrDefault();
        }

        public decimal TodayTotalPrice()
        {
            using var context = new SignalRContext();
            DateTime NowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            return context.Orders.Where(x => x.OrderDate == NowDate && x.Description == "Hesap Kapatıldı").Sum(y => y.TotalOrderPrice);
        }

        public int TotalOrderNumber()
        {
           using var context = new SignalRContext();
            return context.Orders.Count();
        }


    }
}
