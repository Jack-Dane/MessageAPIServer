using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageAppServer.Controllers;
using MessageAppServer.Models;
using System.Web.Http.Results;
using System.Threading.Tasks;
using MessageAppServer.DAL;
using Moq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace MessageAppServerTests.Tests
{
    [TestClass]
    public class MessageControllerTest
    {
        [TestMethod]
        public async Task GetMessages_ShouldReturnMessages()
        {
            int messageId = 10;
            var repo = new Mock<IMessageRepository>();
            repo.Setup(repo => repo.FindMessageAsync(messageId)).Returns(Task.FromResult(GetMessage()));

            var controller = new MessagesController(repo.Object);
            var actionResult = await controller.GetMessage(messageId);
            var result = actionResult.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(GetMessage().MessageId, result.MessageId);
        }

        [TestMethod]
        public void UpdateMessage_ShouldUpdateMessage()
        {

        }

        [TestMethod]
        public void CreateMessage_ShouldCreateMessage()
        {

        }

        [TestMethod]
        public void DeleteMessage_ShouldDeleteMessage()
        {

        }

        private Message GetMessage()
        {
            return new Message() { MessageId = 10 };
        }
    }
}
