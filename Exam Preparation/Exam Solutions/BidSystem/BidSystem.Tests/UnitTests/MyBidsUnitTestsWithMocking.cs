using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using BidSystem.Data.Models;
using BidSystem.Data.Repositories;
using BidSystem.Data.UnitOfWork;
using BidSystem.RestServices.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BidSystem.Tests.UnitTests
{
    [TestClass]
    public class MyBidsUnitTestsWithMocking
    {
        [TestMethod]
        public void GetMyBids_ExistingBids_ShouldReturn_200OK()
        {
            var fakeBids = new List<Bid>()
                {
                    //Arrange
                    new Bid()
                    {
                        Id = 1,
                        DateCreated = DateTime.Now.AddDays(-1),
                        OfferedPrice = 123.89m,
                        OfferId = 6,
                    },
                    new Bid()
                    {
                        Id = 2,
                        DateCreated = DateTime.Now.AddDays(-5),
                        OfferedPrice = 234.67m,
                        OfferId = 6
                    }
                };

            var mockRepository = new Mock<IRepository<Bid>>();
            mockRepository
                .Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return fakeBids.FirstOrDefault(f => f.Id == id);
                });

            var mockUnitOfWork = new Mock<IBidSystemData>();
            mockUnitOfWork.Setup(u => u.Bids).Returns(mockRepository.Object);

            //Act

            var controller = new BidsController(mockUnitOfWork.Object);
            SetupController(controller);

            var response = controller.ListUserBids().ExecuteAsync(CancellationToken.None).Result;

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            mockUnitOfWork.Verify(c => c.SaveChanges(), Times.Once);
        }

        private void SetupController(ApiController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}
