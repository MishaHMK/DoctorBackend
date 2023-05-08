using Doctor.BLL.Services;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using DoctorWebApi.Helper;
using Doctor.DataAcsess.Interfaces;
using Moq;
using Xunit;
using AutoMapper;

namespace Doctor.XUnit
{
    public class MessageServiceTests
    {
        private readonly Mock<IMessageRepository> _messageRepository;
        private readonly Mock<IMapper> _mapper;

        public MessageServiceTests()
        {
            _messageRepository = new Mock<IMessageRepository>();
            _mapper = new Mock<IMapper>();
        }

        private MessageService CreateMessageService()
        {
            _messageRepository.Setup(r => r.AddMessage(new Message()));

            _messageRepository.Setup(r => r.DeleteMessage(new Message()));

            _messageRepository.Setup(r => r.GetMessage(It.IsAny<int>()))
                           .ReturnsAsync(new Message());

            _messageRepository.Setup(r => r.GetMessageThread(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync(CreateMessageEnumerable);

            _messageRepository.Setup(r => r.SaveAllAsync())
                           .ReturnsAsync(It.IsAny<bool>());

            _messageRepository.Setup(r => r.GetSender(CreateMessage))
                            .ReturnsAsync(new User());

            _messageRepository.Setup(r => r.GetRecepient(CreateMessage))
                            .ReturnsAsync(new User());

            return new MessageService(
                _messageRepository.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task CreateMessage_ReturnsMessage()
        {
            MessageService msgService = CreateMessageService();

            var result = await msgService.CreateMessage(CreateMessage);

            Assert.NotNull(result);
            Assert.IsType<Message>(result);
        }

        [Fact]
        public async Task GetMessagesThread_ReturnsEnumerable()
        {
            MessageService msgService = CreateMessageService();

            var result = await msgService.GetMessagesThread(It.IsAny<string>(), It.IsAny<string>());

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<MessageDTO>>(result);
        }

        [Fact]
        public async Task GetMessageById_ReturnsMessage()
        {
            MessageService msgService = CreateMessageService();

            var result = await msgService.GetMessage(It.IsAny<int>());

            Assert.NotNull(result);
            Assert.IsType<Message>(result);
        }

        [Fact]
        public async Task SaveAllAsync_ReturnsBool()
        {
            MessageService msgService = CreateMessageService();

            var result = await msgService.SaveAllAsync();

            Assert.IsType<bool>(result);
        }


        private IEnumerable<Message> CreateMessageEnumerable => new List<Message>()
        {
            new Message(),
            new Message()
        }.AsAsyncQueryable();

        private CreateMessage CreateMessage = new CreateMessage { Content = "som", RecipientName = "Mykhailo", SenderName = "Lol" };
    }
}
