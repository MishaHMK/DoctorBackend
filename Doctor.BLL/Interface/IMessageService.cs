using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.BLL.Interface
{
    public interface IMessageService
    {
        Task<Message> CreateMessage(CreateMessage createParams);
        Task<PagedList<MessageDTO>> GetMessages(MessageParams messageParams, string userId);
        Task<IEnumerable<MessageDTO>> GetMessagesThread(string un_send, string un_rec);
        Task<Message> GetMessage(int id);
        Task DeleteMessageAsync(int id, string un_send);
        Task<bool> SaveAllAsync();
    }
}
