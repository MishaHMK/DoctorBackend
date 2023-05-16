using AutoMapper;
using AutoMapper.QueryableExtensions;
using Doctor.BLL.Interface;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using Doctor.DataAcsess.Interfaces;

namespace Doctor.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IMapper mapper) {
            _messageRepository = messageRepository;
            _mapper = mapper;   
        }

        public async Task<Message> CreateMessage(CreateMessage createParams)
        {
            var sender = await _messageRepository.GetSender(createParams);
            var recepient = await _messageRepository.GetRecepient(createParams);

            if (sender != null && recepient != null)
            {
                var message = new Message
                {
                    Sender = sender,
                    Recipient = recepient,
                    SenderUserName = sender.Name,
                    RecipientUserName = recepient.Name,
                    SenderId = sender.Id,   
                    RecipientId = recepient.Id,
                    Content = createParams.Content
                };

                _messageRepository.AddMessage(message);
                return message;
            }
            return null;
        }

        public async Task<bool> SaveAllAsync()
        {
            if (await _messageRepository.SaveAllAsync())
            {
                return true;
            }
                return false;
        }

        public async Task<PagedList<MessageDTO>> GetMessages(MessageParams messageParams, string userId)
        {
            var query = await _messageRepository.GetMessagesQueryForUser();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientId == userId),
                "Outbox" => query.Where(u => u.SenderId == userId),
                _ => query.Where(u => u.RecipientId == userId && u.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDTO>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize, query.Count());
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesThread(string id_send, string id_rec)
        {

            var messages = await _messageRepository.GetMessageThread(id_send, id_rec);

            var unreadMessages = messages.Where(m => m.DateRead == null &&
                                  m.RecipientId == id_send).ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.Now;
                }

                await _messageRepository.SaveAsync();
            }

            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }

        //public async Task<IEnumerable<MessageDTO>> GetMessagesThread(string un_send, string un_rec)
        //{

        //    var messages = await _messageRepository.GetMessageThread(un_send, un_rec);

        //    var unreadMessages = messages.Where(m => m.DateRead == null &&
        //                          m.RecipientUserName == un_send).ToList();

        //    if (unreadMessages.Any())
        //    {
        //        foreach (var message in unreadMessages)
        //        {
        //            message.DateRead = DateTime.Now;
        //        }

        //        await _messageRepository.SaveAsync();
        //    }

        //    return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        //}


        public async Task<Message> GetMessage(int id)
        {
            return await _messageRepository.GetMessage(id); 
        }

        public async Task DeleteMessageAsync(int id, string id_send)
        {
            var message = await GetMessage(id);

            if (message.Sender.Id == id_send) message.SenderDeleted = true;

            if (message.Recipient.Id == id_send) message.RecepientDeleted = true;

            if (message.SenderDeleted || message.RecepientDeleted)

            _messageRepository.DeleteMessage(message);
        }

        //public async Task DeleteMessageAsync(int id, string un_send)
        //{
        //    var message = await GetMessage(id);

        //    if (message.Sender.Name == un_send) message.SenderDeleted = true;

        //    if (message.Recipient.Name == un_send) message.RecepientDeleted = true;

        //    if (message.SenderDeleted || message.RecepientDeleted)

        //        _messageRepository.DeleteMessage(message);
        //}
    }
}
