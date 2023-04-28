using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;

namespace Doctor.DataAcsess.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<IQueryable<Message>> GetMessagesQueryForUser();
        Task<IEnumerable<Message>> GetMessageThread(string currentUserName, string recipientUserName);
        Task<bool> SaveAllAsync();
        Task SaveAsync();
        Task<User?> GetSender(CreateMessage createParams);
        Task<User?> GetRecepient(CreateMessage createParams);
    }
}
