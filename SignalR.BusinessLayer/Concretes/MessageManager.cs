using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Concretes
{
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;
        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public void TAdd(Message entity)
        {
            _messageDal.Add(entity);
           
        }

        public void TDelete(Message entity)
        {
            _messageDal.Delete(entity);
        }

        public  async Task<IEnumerable<Message>> TGetAllMessages()
        {
           return await _messageDal.GetAllMessages();
        }

        public Message TGetById(int id)
        {
            return _messageDal.GetById(id);
        }

        public async Task<IEnumerable<Message>> TGetConversation(int userId1, int userId2)
        {
            return await _messageDal.GetConversation(userId1, userId2);
        }

        public List<Message> TGetListAll()
        {
            return _messageDal.GetListAll();

        }

        public async Task<Message> TGetMessageById(int messageId)
        {
           return await _messageDal.GetMessageById(messageId);
        }

        public async Task<IEnumerable<Message>> TGetMessagesByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _messageDal.GetMessagesByDateRange(startDate, endDate);
        }

        public async Task<IEnumerable<Message>> TGetMessagesByUserId(int userId)
        {
            return await _messageDal.GetMessagesByUserId(userId);

        }

        public void TUpdate(Message entity)
        {
            _messageDal.Update(entity);
        }
    }
}
