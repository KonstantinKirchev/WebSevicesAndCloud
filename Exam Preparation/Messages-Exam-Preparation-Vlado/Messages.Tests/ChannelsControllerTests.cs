using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessagesRestService.Controllers;
using System.Web.Http;
using System.Net;
using System.Linq;
using System.Net.Http;

namespace Messages.Tests
{
    [TestClass]
    public class ChannelsControllerTests
    {
        [TestMethod]
        public void GetAllChannelsShouldReturnOkResponse()
        {
            var controller = new ChannelsController();

            var response = controller.GetChannels();

            Assert.IsNotNull(response);
        }
    }
}
