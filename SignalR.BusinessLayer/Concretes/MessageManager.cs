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
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;
        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        // Asenkron ekleme metodu
        public async Task TAddAsync(Message entity)
        {
            await _messageDal.AddAsync(entity);
            await _messageDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // Asenkron silme metodu
        public async Task TDeleteAsync(Message entity)
        {
            await _messageDal.DeleteAsync(entity);
            await _messageDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // Tüm mesajları asenkron getirme metodu
        public async Task<IEnumerable<Message>> TGetAllMessagesAsync() // Metot adı Async ile güncellendi
        {
            return await _messageDal.GetAllMessages(); // Eğer DAL'da GetAllMessagesAsync varsa onu çağırın
        }

        // ID'ye göre asenkron getirme metodu
        public async Task<Message> TGetByIdAsync(int id)
        {
            return await _messageDal.GetByIdAsync(id);
        }

        // Kullanıcılar arası konuşmayı asenkron getirme metodu
        public async Task<IEnumerable<Message>> TGetConversationAsync(int userId1, int userId2) // Metot adı Async ile güncellendi
        {
            return await _messageDal.GetConversation(userId1, userId2); // Eğer DAL'da GetConversationAsync varsa onu çağırın
        }

        // Tüm mesajların listesini asenkron getirme metodu
        public async Task<List<Message>> TGetListAllAsync()
        {
            return await _messageDal.GetListAllAsync();
        }

        // Belirli ID'ye sahip mesajı asenkron getirme metodu
        public async Task<Message> TGetMessageByIdAsync(int messageId) // Metot adı Async ile güncellendi
        {
            return await _messageDal.GetMessageByIdAsync(messageId); // Eğer DAL'da GetMessageByIdAsync varsa onu çağırın
        }

        // Tarih aralığına göre mesajları asenkron getirme metodu
        public async Task<IEnumerable<Message>> TGetMessagesByDateRangeAsync(DateTime startDate, DateTime endDate) // Metot adı Async ile güncellendi
        {
            return await _messageDal.GetMessagesByDateRange(startDate, endDate); // Eğer DAL'da GetMessagesByDateRangeAsync varsa onu çağırın
        }

        // Kullanıcı ID'sine göre mesajları asenkron getirme metodu
        public async Task<IEnumerable<Message>> TGetMessagesByUserIdAsync(int userId) // Metot adı Async ile güncellendi
        {
            return await _messageDal.GetMessagesByUserId(userId); // Eğer DAL'da GetMessagesByUserIdAsync varsa onu çağırın
        }

        // Asenkron güncelleme metodu
        public async Task TUpdateAsync(Message entity)
        {
            await _messageDal.UpdateAsync(entity);
            await _messageDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // Değişiklikleri asenkron kaydetme metodu
        public async Task TSaveChangesAsync()
        {
            await _messageDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }
    }
}
