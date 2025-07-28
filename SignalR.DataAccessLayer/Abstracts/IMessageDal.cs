using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IMessageDal : IGenericDal<Message>
    {
        Task<IEnumerable<Message>> GetAllMessages();
        Task<IEnumerable<Message>> GetMessagesByUserId(int userId);
        Task<Message> GetMessageById(int messageId);
        Task<IEnumerable<Message>> GetMessagesByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Message>> GetConversation(int userId1, int userId2);
    }
}
