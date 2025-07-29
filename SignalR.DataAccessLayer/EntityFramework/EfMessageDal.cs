using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfMessageDal : GenericRepository<Message>, IMessageDal
    {
        private readonly SignalRContext _context;

        public EfMessageDal(SignalRContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllMessages() // Metot adı Async'siz olarak bırakıldı
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetConversation(int userId1, int userId2) // Metot adı Async'siz olarak bırakıldı
        {
            return await _context.Messages
                       .Where(m => (m.SenderUserId == userId1 && m.ReceiverUserId == userId2) ||
                                   (m.SenderUserId == userId2 && m.ReceiverUserId == userId1))
                       .OrderBy(m => m.Timestamp)
                       .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _context.Messages.FindAsync(messageId);
        }

        public async Task<IEnumerable<Message>> GetMessagesByDateRange(DateTime startDate, DateTime endDate) // Metot adı Async'siz olarak bırakıldı
        {
            return await _context.Messages
                       .Where(m => m.Timestamp >= startDate && m.Timestamp <= endDate)
                       .OrderByDescending(m => m.Timestamp)
                       .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesByUserId(int userId) // Metot adı Async'siz olarak bırakıldı
        {
            return await _context.Messages
                .Where(m => m.SenderUserId == userId || m.ReceiverUserId == userId)
                .ToListAsync();
        }
    }
}
