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
        public int TNotificationCountByStatusFalse();
        public void TNotificationStatusFalse(int id);
        public void TNotificationStatusTrue(int id);
    }
}
