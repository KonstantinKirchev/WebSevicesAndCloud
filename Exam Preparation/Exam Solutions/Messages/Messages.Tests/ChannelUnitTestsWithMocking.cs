
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Routing;
using Messages.Data.Models;
using Messages.Data.Repositories;
using Messages.Data.UnitOfWork;
using Messages.RestServices.Controllers;
using Messages.Tests.Mocks;
using Messages.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Messages.Tests
{
    [TestClass]
    public class ChannelUnitTestsWithMocking
    {
        [TestMethod]
        public void GetChannelById_ExistingChannel_ShoulReturn200_OK()
        {
            //    var fakeChannels = new List<Channel>()
            //        {
            //            //Arrange
            //            new Channel()
            //            {
            //                Id = 1,
            //                Name = "CNN"
            //            },
            //            new Channel()
            //            {
            //                Id = 2,
            //                Name = "BTV"
            //            }
            //        };

            //    var mockRepository = new Mock<IRepository<Channel>>();
            //    mockRepository
            //        .Setup(r => r.Find(It.IsAny<int>()))
            //        .Returns((int id) =>
            //        {
            //            return fakeChannels.FirstOrDefault(f => f.Id == id);
            //        });


            //    var mockUnitOfWork = new Mock<IMessagesUnitOfWork>();
            //    mockUnitOfWork.Setup(u => u.Channels).Returns(mockRepository.Object);

            //    //Act

            //    var controller = new ChannelsController(mockUnitOfWork.Object);
            //    SetupController(controller);

            //    var response = controller.GetChannel(2).ExecuteAsync(CancellationToken.None).Result;

            //    //Assert

            //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //    //mockUnitOfWork.Verify(c => c.SaveChanges(), Times.Once);
            //    var channel = response.Content.ReadAsAsync<ChannelModel>().Result;
            //    Assert.AreEqual(1, channel.Id);
            //    Assert.AreEqual("CNN", channel.Name);
            //}

            //private void SetupController(ApiController controller)
            //{
            //    controller.Request = new HttpRequestMessage();
            //    controller.Configuration = new HttpConfiguration();
            //}
            //Arrange -> create mocks
            IMessagesUnitOfWork mockedUnitOfWork = new MessagesDataMock();

            var channelsMock = mockedUnitOfWork.Channels;

            channelsMock.Add(new Channel()
            {
                Id = 5,
                Name = "CNN"
            });

            channelsMock.Add(new Channel()
            {
                Id = 6,
                Name = "Russia Today"
            });

            //Act -> invoke channelsController
            var channelsController = new ChannelsController(mockedUnitOfWork);
            this.SetupControllerForTesting(channelsController, "channels");
            var httpResult = channelsController.GetChannel(12).ExecuteAsync(new CancellationToken()).Result;

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, httpResult.StatusCode);
        }


        [TestMethod]
        public void GetChannelById_ExistingChannel_ShoulReturn200OK()
        {
            //Arrange
            IMessagesUnitOfWork mockedUnitOfWork = new MessagesDataMock();

            var channelsMock = mockedUnitOfWork.Channels;

            channelsMock.Add(new Channel()
            {
                Id = 5,
                Name = "CNN"
            });

            channelsMock.Add(new Channel()
            {
                Id = 6,
                Name = "Russia Today"
            });

            //Act
            var channelsController = new ChannelsController(mockedUnitOfWork);
            this.SetupControllerForTesting(channelsController, "channels");
            var httpResult = channelsController.GetChannel(6).ExecuteAsync(new CancellationToken()).Result;


            //Assert
            Assert.AreEqual(HttpStatusCode.OK, httpResult.StatusCode);

            var channel = httpResult.Content.ReadAsAsync<ChannelModel>().Result;
            Assert.AreEqual(6, channel.Id);
            Assert.AreEqual("Russia Today", channel.Name);
        }

        private void SetupControllerForTesting(ApiController controller, string controllerName)
        {
            string serverUrl = "http://sample-url.com";

            // Setup the Request object of the controller
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(serverUrl)
            };
            controller.Request = request;

            // Setup the configuration of the controller
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
            controller.Configuration = config;

            // Apply the routes to the controller
            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary
                    {
                        {"controller", controllerName}
                    });
        }
    }
}

