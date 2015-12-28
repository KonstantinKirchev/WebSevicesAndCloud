using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using Messages.Data;
using Messages.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Messages.Tests
{
    [TestClass]
    public class ChannelIntegrationTests
    {
        private MessagesDbContext db = new MessagesDbContext();

        [TestMethod]
        public void DeleteChannel_NonExisting_ShouldReturn_404NotFound()
        {
            TestingEngine.CleanDatabase();

            var channelIdNonExisting = 7;

            // Act -> delete the channel
            var httpDeleteResponse = TestingEngine.HttpClient.DeleteAsync(
                "/api/channels/" + channelIdNonExisting).Result;

            // Assert -> HTTP status code is 404 Not Found
            Assert.AreEqual(HttpStatusCode.NotFound, httpDeleteResponse.StatusCode);
            Assert.AreEqual(0, TestingEngine.GetChannelsCountFromDb());
        }

        [TestMethod]
        public void DeleteChannel_ExistingChannel_ShouldReturn_200OK()
        {
            //Arrange
            TestingEngine.CleanDatabase();

            var channel = new Channel(){Name = "nov kanal"};
           
            db.Channels.Add(channel);
            db.SaveChanges();

            // Act -> delete the channel
            var httpDeleteResponse = TestingEngine.HttpClient.DeleteAsync(
                "/api/channels/" + channel.Id).Result;

            // Assert -> HTTP status code is 200 OK
            Assert.AreEqual(HttpStatusCode.OK, httpDeleteResponse.StatusCode);
            
            var response = httpDeleteResponse.Content.ReadAsAsync<Dictionary<string, string>>().Result;
            
            Assert.IsNotNull(response["Message"]);
            Assert.AreEqual(0, db.Channels.Count());
        }

        [TestMethod]
        public void DeleteChannel_ExistingChannelNotEmpty_ShouldReturn_409Conflict()
        {
            //Arrange
            TestingEngine.CleanDatabase();

            var channel = new Channel() { Name = "nov kanal" };
       
            channel.ChannelMessages.Add(new ChannelMessage()
            {
                Id = 1233,
                Sender = null,
                DateSent = DateTime.Now,
                Text = "Hello Dido"
            });

            db.Channels.Add(channel);
            db.SaveChanges();

            // Act -> delete the channel
            var httpDeleteResponse = TestingEngine.HttpClient.DeleteAsync(
                "/api/channels/" + channel.Id).Result;

            // Assert -> HTTP status code is 409 Conflict
            Assert.AreEqual(HttpStatusCode.Conflict, httpDeleteResponse.StatusCode);
      
            Assert.AreEqual(1, db.Channels.Count());
        }
    }
}
