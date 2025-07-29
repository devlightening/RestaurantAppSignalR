using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface INotificationService : IGenericService<Notification>
    {
        Task<int> TNotificationCountByStatusFalse();
        Task<List<Notification>> TGetAllNotificationByFalse();
        Task TNotificationStatusFalse(int id);
        Task TNotificationStatusTrue(int id);
    }
}
