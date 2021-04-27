using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageAppServer.Controllers;
using MessageAppServerTests.MockData;

namespace MessageAppServerTests.Tests
{
    [TestClass]
    public class MessageControllerTest
    {
        [TestMethod]
        public void GetMessages_ShouldReturnMessages()
        {
            var controller = new MessagesController(new TestMessageContext());
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
    }
}
