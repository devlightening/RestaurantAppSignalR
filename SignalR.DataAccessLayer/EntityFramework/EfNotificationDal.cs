using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfNotificationDal : GenericRepository<Notification>, INotificationDal
    {
        private readonly SignalRContext _context;

        public EfNotificationDal(SignalRContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetAllNotificationByFalse()
        {
            return await _context.Notifications.Where(x => x.Status == false).ToListAsync();
        }

        public async Task<int> NotificationCountByStatusFalse()
        {
            return await _context.Notifications.CountAsync(x => x.Status == false);
        }

        public async Task NotificationStatusFalse(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.Status = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task NotificationStatusTrue(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.Status = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
