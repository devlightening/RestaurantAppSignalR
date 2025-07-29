using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface INotificationDal : IGenericDal<Notification>
    {
        Task<int> NotificationCountByStatusFalse();
        Task<List<Notification>> GetAllNotificationByFalse();
        Task NotificationStatusFalse(int id);
        Task NotificationStatusTrue(int id);
    }
}

