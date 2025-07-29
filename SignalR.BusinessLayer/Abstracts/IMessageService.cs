using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IMessageService 
    {
        Task TAddAsync(Message entity);
        Task TDeleteAsync(Message entity);
        Task TUpdateAsync(Message entity);
        Task<Message> TGetByIdAsync(int id);
        Task<List<Message>> TGetListAllAsync();
        public Task<IEnumerable<Message>> TGetAllMessagesAsync();
        public Task<IEnumerable<Message>> TGetMessagesByUserIdAsync(int userId);
        public Task<Message> TGetMessageByIdAsync(int messageId);
        Task<IEnumerable<Message>> TGetMessagesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Message>> TGetConversationAsync(int userId1, int userId2);

    }
}
