using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IMessageService : IGenericService<Message>
    {
        public Task<IEnumerable<Message>> TGetAllMessages();
        public Task<IEnumerable<Message>> TGetMessagesByUserId(int userId);
        public Task<Message> TGetMessageById(int messageId);
        Task<IEnumerable<Message>> TGetMessagesByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Message>> TGetConversation(int userId1, int userId2);




    }
}
