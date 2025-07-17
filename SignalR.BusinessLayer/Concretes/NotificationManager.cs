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
    public class NotificationManager : INotificationService
    {
        private readonly INotificationDal _notificationDal;

        public NotificationManager(INotificationDal notificationDal)
        {
            _notificationDal = notificationDal;
        }

        public void TAdd(Notification entity)
        {
            _notificationDal.Add(entity);
        }

        public void TDelete(Notification entity)
        {
           _notificationDal.Delete(entity);
        }

        public Notification TGetById(int id)
        {
            return _notificationDal.GetById(id);
           
        }

        public List<Notification> TGetListAll()
        {
           return _notificationDal.GetListAll();
        }

        public int TNotificationCountByStatusFalse()
        {
            return _notificationDal.NotificationCountByStatusFalse();
        }

        public void TNotificationStatusFalse(int id)
        {
            _notificationDal.NotificationStatusFalse(id);
        }

        public void TNotificationStatusTrue(int id)
        {
             _notificationDal.NotificationStatusTrue(id); 
        }

        public void TUpdate(Notification entity)
        {
           _notificationDal.Update(entity);
        }
    }
}
