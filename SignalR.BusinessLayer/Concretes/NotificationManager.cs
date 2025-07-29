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

        public async Task TAddAsync(Notification entity)
        {
            await _notificationDal.AddAsync(entity);
            await _notificationDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task TDeleteAsync(Notification entity)
        {
            await _notificationDal.DeleteAsync(entity);
            await _notificationDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task<List<Notification>> TGetAllNotificationByFalse()
        {
            // Bu metot için INotificationDal'da GetAllNotificationByFalseAsync() olduğunu varsayıyorum.
            // Eğer yoksa, _notificationDal.GetListAllAsync() çağrılıp filtreleme yapılabilir.
            return await _notificationDal.GetAllNotificationByFalse();
        }

        public async Task<Notification> TGetByIdAsync(int id)
        {
            return await _notificationDal.GetByIdAsync(id);
        }

        public async Task<List<Notification>> TGetListAllAsync()
        {
            return await _notificationDal.GetListAllAsync();
        }

        public async Task<int> TNotificationCountByStatusFalse()
        {
            // Bu metot için INotificationDal'da NotificationCountByStatusFalseAsync() olduğunu varsayıyorum.
            return await _notificationDal.NotificationCountByStatusFalse();
        }

        public async Task TNotificationStatusFalse(int id)
        {
            // ID'ye göre bildirimi bul
            var notification = await _notificationDal.GetByIdAsync(id);
            if (notification != null)
            {
                notification.Status = false; // Durumu pasif yap
                await _notificationDal.UpdateAsync(notification); // Güncelle
                await _notificationDal.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            // Hata yönetimi eklenebilir (örn: bildirim bulunamazsa loglama)
        }

        public async Task TNotificationStatusTrue(int id)
        {
            // ID'ye göre bildirimi bul
            var notification = await _notificationDal.GetByIdAsync(id);
            if (notification != null)
            {
                notification.Status = true; // Durumu aktif yap
                await _notificationDal.UpdateAsync(notification); // Güncelle
                await _notificationDal.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            // Hata yönetimi eklenebilir
        }

        public async Task TSaveChangesAsync()
        {
            await _notificationDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }

        public async Task TUpdateAsync(Notification entity)
        {
            await _notificationDal.UpdateAsync(entity);
            await _notificationDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }
    }
}
