using Moq;
using NUnit.Framework;
using MessageAppServer.Models;
using MessageAppServer.DAL;
using System.Threading.Tasks;
using MessageAppServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MessageAppTests.Controllers
{
    public class Tests
    {
        private Mock<IMessageRepository> _messageRepo;

        [SetUp]
        public void Setup()
        {
            _messageRepo = new Mock<IMessageRepository>();
        }

        [Test]
        public async Task GetMessages_ShouldReturnAllMessages()
        {
            List<Message> allMessages = GetMessages();
            _messageRepo.Setup(repo => repo.GetMessageBasedOnUser(null))
                .Returns(Task.FromResult(allMessages));

            var controller = new MessagesController(_messageRepo.Object);
            var actionResult = await controller.GetMessages();
            var result = actionResult.Value;

            Assert.IsInstanceOf<List<Message>>(result);
            List<Message> listResult = (List<Message>)result;
            Assert.AreEqual(allMessages.Count, listResult.Count);

            for (int i=0; i<allMessages.Count; i++)
            {
                Assert.AreEqual(allMessages[i].MessageId, listResult[i].MessageId);
            }
        }

        [Test]
        public async Task GetMessage_ShouldReturnMessage()
        {
            int messageId = GetMessage().MessageId;
            _messageRepo.Setup(repo => repo.FindMessageAsync(messageId))
                .Returns(Task.FromResult(GetMessage()));

            var controller = new MessagesController(_messageRepo.Object);
            var actionResult = await controller.GetMessage(messageId);
            var result = actionResult.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(GetMessage().MessageId, result.MessageId);
        }

        [Test]
        public async Task GetMessages_ShouldReturnNull()
        {
            int messageId = GetMessage().MessageId;
            _messageRepo.Setup(repo => repo.FindMessageAsync(messageId))
                .Returns(Task.FromResult((Message)null));

            var controller = new MessagesController(_messageRepo.Object);
            var actionResult = await controller.GetMessage(messageId);
            var result = actionResult.Value;

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateMessage_ShouldUpdate()
        {
            Message beforeMessage = GetMessage();

            var controller = new MessagesController(_messageRepo.Object);
            await controller.PutMessage(beforeMessage.MessageId, beforeMessage);

            _messageRepo.Verify(mock => mock.MarkAsModified(beforeMessage), Times.Once());
            _messageRepo.Verify(mock => mock.SaveChangesAsync(), Times.Once());
        }

        [Test]
        public async Task UpdateMessage_ShouldReturnBadRequest()
        {
            Message beforeMessage = GetMessage();

            var controller = new MessagesController(_messageRepo.Object);
            // add one to the messageId, so it will always be incorrect
            var response = await controller.PutMessage(beforeMessage.MessageId + 1, beforeMessage);

            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public async Task CreateMessage_ShouldCreateMessage()
        {
            Message newMessage = GetMessage();

            var controller = new MessagesController(_messageRepo.Object);
            await controller.PostMessage(newMessage);

            _messageRepo.Verify(mock => mock.AddMessage(newMessage), Times.Once());
            _messageRepo.Verify(mock => mock.SaveChangesAsync(), Times.Once());
        }

        [Test]
        public async Task DeleteMessage_ShouldDeleteMessage()
        {
            Message messageToDelete = GetMessage();
            _messageRepo.Setup(repo => repo.FindMessageAsync(messageToDelete.MessageId))
                .Returns(Task.FromResult(messageToDelete));

            var controller = new MessagesController(_messageRepo.Object);
            await controller.DeleteMessage(messageToDelete.MessageId);

            _messageRepo.Verify(mock => mock.FindMessageAsync(messageToDelete.MessageId), Times.Once());
            _messageRepo.Verify(mock => mock.RemoveMessage(messageToDelete), Times.Once());
            _messageRepo.Verify(mock => mock.SaveChangesAsync(), Times.Once());
        }

        [Test]
        public async Task DeleteMessage_ShouldNotBeFound()
        {
            Message messageToDelete = GetMessage();
            _messageRepo.Setup(repo => repo.FindMessageAsync(messageToDelete.MessageId))
                .Returns(Task.FromResult((Message)null));

            var controller = new MessagesController(_messageRepo.Object);
            var response = await controller.DeleteMessage(messageToDelete.MessageId);

            Assert.IsInstanceOf<NotFoundResult>(response);
        }

        private Message GetMessage()
        {
            return new Message() { MessageId = 10 };
        }

        private List<Message> GetMessages()
        {
            return new List<Message>()
            {
                new Message() {MessageId = 1},
                new Message() {MessageId = 2}
            };
        }
    }
}