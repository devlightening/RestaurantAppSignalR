using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfNotificationDal : GenericRepository<Notification>, INotificationDal
    {
        public EfNotificationDal(SignalRContext context) : base(context)
        {
        }

        public int NotificationCountByStatusFalse()
        {
            using var context = new SignalRContext();
            return context.Notifications.Count(a => a.Status == false);
        }

        public void NotificationStatusFalse(int id)
        {
            using var context = new SignalRContext();
            var notification = context.Notifications.Where(a=>a.NotificationId == id).FirstOrDefault();
            notification.Status = false;
            context.SaveChanges();
        }

        public void NotificationStatusTrue(int id)
        {
            using var context = new SignalRContext();
            var notification = context.Notifications.Where(a => a.NotificationId == id).FirstOrDefault();
            notification.Status = true;
            context.SaveChanges();
        }
    }
}
