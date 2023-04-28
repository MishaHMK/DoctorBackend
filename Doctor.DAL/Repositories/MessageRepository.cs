
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using Doctor.DataAcsess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Doctor.DataAcsess.Interfaces
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext db) : base(db)
        {

        }

        public void AddMessage(Message message)
        {
            dbSet.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            dbSet.Remove(message);
        }

        public async Task<User?> GetSender(CreateMessage createParams)
        {
            var sender = await _db.Users.SingleOrDefaultAsync(x => x.Name == createParams.SenderName);
            if (sender != null) {
                return sender;
            }
            return null;
        }

        public async Task<User?> GetRecepient(CreateMessage createParams)
        {
            var recepient = await _db.Users.SingleOrDefaultAsync(x => x.Name == createParams.RecipientName);
            if (recepient != null)
            {
                return recepient;
            }
            return null;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await dbSet.Where(u => u.Id == id).Include(r => r.Recipient).Include(s => s.Sender).FirstOrDefaultAsync(); 
        }

        public async Task<IQueryable<Message>> GetMessagesQueryForUser()
        {
            var query = dbSet.OrderByDescending(m => m.MessageSent).AsQueryable();
            return query;
        }

        public async Task<IEnumerable<Message>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var messages = await dbSet
                           .Where(m => m.RecipientUserName == currentUserName &&
                                  m.RecepientDeleted == false &&
                                  m.SenderUserName == recipientUserName ||
                                  m.RecipientUserName == recipientUserName &&
                                  m.SenderUserName == currentUserName && 
                                  m.SenderDeleted == false)
                           .OrderBy(m => m.MessageSent) 
                           .ToListAsync();

            return messages;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
