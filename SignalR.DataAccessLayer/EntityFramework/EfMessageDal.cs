using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfMessageDal : GenericRepository<Message>, IMessageDal
    {
        private readonly SignalRContext _context;

        public EfMessageDal(SignalRContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetConversation(int userId1, int userId2)
        {
            return await _context.Messages
                   .Where(m => (m.SenderUserId == userId1 && m.ReceiverUserId == userId2) ||
                               (m.SenderUserId == userId2 && m.ReceiverUserId == userId1))
                   .OrderBy(m => m.Timestamp)
                   .ToListAsync();
        }

        public async Task<Message> GetMessageById(int messageId)
        {
            return await _context.Messages.FindAsync(messageId);
        }

        public async Task<IEnumerable<Message>> GetMessagesByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Messages
                 .Where(m => m.Timestamp >= startDate && m.Timestamp <= endDate)
                 .OrderByDescending(m => m.Timestamp)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesByUserId(int userId)
        {
            return await _context.Messages
                .Where(m => m.SenderUserId == userId || m.ReceiverUserId == userId)
                .ToListAsync();
        }

    }
}
