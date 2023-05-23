using Doctor.BLL.Interface;
using Doctor.DataAcsess.Entities;
using Doctor.DataAcsess.Helpers;
using DoctorWebApi.Controllers;
using DoctorWebApi.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NPOI.OpenXmlFormats.Wordprocessing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Doctor.NUnit.Controllers.MessagesControllerTests;

namespace Doctor.NUnit.Controllers
{
    internal class MessagesControllerTests
    {
        private readonly Mock<IMessageService> _msgService;

        public MessagesControllerTests()
        {
            _msgService = new Mock<IMessageService>();
        }

        private MessagesController MessagesController =>
        new MessagesController(
           _msgService.Object
        );

        [Test]
        public async Task CreateMessage_ReturnsOk()
        {
            // Arrange
            _msgService.Setup(x => x.CreateMessage(CreateMessageOk)).ReturnsAsync(CreatedMessage);

            // Act
            var result = await MessagesController.CreateMessage(CreateMessageOk);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CreateMessage_ReturnsBad()
        {
            // Arrange
            _msgService.Setup(x => x.CreateMessage(CreateMessageBad)).ReturnsAsync(It.IsAny<Message>());

            // Act
            var result = await MessagesController.CreateMessage(CreateMessageBad);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task CreateMessage_ReturnsNotFound()
        {
            // Arrange
            _msgService.Setup(x => x.CreateMessage(CreateMessageNotFound)).ReturnsAsync(It.IsAny<Message>());

            // Act
            var result = await MessagesController.CreateMessage(CreateMessageNotFound);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetMessagesThread_ReturnsOk()
        {
            // Arrange
            _msgService.Setup(x => x.GetMessagesThread(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<IEnumerable<MessageDTO>>);

            // Act
            var result = await MessagesController.GetMessagesThread(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }


        [Test]
        public async Task GetMessagesForUser_ReturnsOk()
        {
            // Arrange
            var MessageParams = new MessageParams { Container = "Inbox", PageNumber = 1, PageSize = 3 };
            var id = "29f40225-fc3b-4ee3-8758-baae8aaf4300";
            var PagedDTOList = PagedList<MessageDTO>.CreateAsync(MsgDtoQueryable, MessageParams.PageNumber, MessageParams.PageSize, MsgDtoQueryable.Count());
                               
            _msgService.Setup(x => x.GetMessages(MessageParams, id))
                       .Returns(PagedDTOList);

            // Act
            var result = await MessagesController.GetMessagesForUser(MessageParams, id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteMessage_ReturnsOk()
        {
            // Arrange
            _msgService.Setup(x => x.DeleteMessageAsync(It.IsAny<int>(), It.IsAny<string>()));

            // Act
            var result = await MessagesController.DeleteMessage(It.IsAny<int>(), It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }



        private CreateMessage CreateMessageOk = new CreateMessage { Content = "som", RecipientName = "Mykhailo", SenderName = "Lol", RecipientId = "Ro", SenderId = "So" };
        private CreateMessage CreateMessageBad = new CreateMessage { Content = "som", RecipientName = "So", SenderName = "So" };
        private CreateMessage CreateMessageNotFound = new CreateMessage { Content = "som", RecipientId = null, SenderId = "So" };

        private IQueryable<MessageDTO> MsgDtoQueryable => new List<MessageDTO>()
        {
            new MessageDTO(),
            new MessageDTO()
        }.AsAsyncQueryable();


        private Message CreatedMessage = new Message { Id = 1, Content = "test", 
                                                       SenderId = "f", SenderUserName = "fff", Sender = It.IsAny<User>(),
                                                       RecipientId = "s", RecipientUserName = "sss", Recipient = It.IsAny<User>()
        };  
    }
}

