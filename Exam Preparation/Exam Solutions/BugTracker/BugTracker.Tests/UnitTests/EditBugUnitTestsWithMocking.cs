using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;
using BugTracker.Data.UnitOfWork;
using BugTracker.RestServices.Controllers;
using BugTracker.RestServices.Models.BindingModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BugTracker.Tests.UnitTests
{
    [TestClass]
    public class EditBugUnitTestsWithMocking
    {
        [TestMethod]
        public void EditingExistingBug_ShouldChangeOnlySentData()
        {
            var fakeBugs = new List<Bug>()
                {
                    //Arrange
                    new Bug()
                    {
                        Id = 1,
                        Title = "Bug #1",
                        Description = "Bug Description",
                        DateCreated = DateTime.Now.AddDays(-1)
                    },
                    new Bug()
                    {
                        Id = 1,
                        Title = "Bug #2",
                        Description = "Bug Description #2",
                        DateCreated = DateTime.Now.AddDays(-5)
                    }
                };

            var mockRepository = new Mock<IRepository<Bug>>();
            mockRepository
                .Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return fakeBugs.FirstOrDefault(f => f.Id == id);
                });

            var mockUnitOfWork = new Mock<IBugsUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Bugs).Returns(mockRepository.Object);

            var newTitle = "Changed " + DateTime.Now.Ticks;

            var model = new EditBugBindingModel()
            {
                Title = newTitle
            };

            var oldDescription = fakeBugs[0].Description;
            var oldStatus = fakeBugs[0].Status;

            //Act

            var controller = new BugsController(mockUnitOfWork.Object);
            SetupController(controller);

            var response = controller.PatchBug(1, model).ExecuteAsync(CancellationToken.None).Result;

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            mockUnitOfWork.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(oldDescription, fakeBugs[0].Description);
            Assert.AreEqual(oldStatus, fakeBugs[0].Status);
            Assert.AreEqual(newTitle, fakeBugs[0].Title);
        
        }

        [TestMethod]
        public void EditingNonExistingBug_ShouldReturn404_NotFound()
        {
            var fakeBugs = new List<Bug>()
                {
                    //Arrange
                    new Bug()
                    {
                        Id = 1,
                        Title = "Bug #1",
                        Description = "Bug Description",
                        DateCreated = DateTime.Now.AddDays(-1)
                    },
                    new Bug()
                    {
                        Id = 1,
                        Title = "Bug #2",
                        Description = "Bug Description #2",
                        DateCreated = DateTime.Now.AddDays(-5)
                    }
                };

            var mockRepository = new Mock<IRepository<Bug>>();
            mockRepository
                .Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return fakeBugs.FirstOrDefault(f => f.Id == id);
                });

            var mockUnitOfWork = new Mock<IBugsUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Bugs).Returns(mockRepository.Object);

            var newTitle = "Changed " + DateTime.Now.Ticks;

            var model = new EditBugBindingModel()
            {
                Title = newTitle
            };

            //Act

            var controller = new BugsController(mockUnitOfWork.Object);
            SetupController(controller);

            var response = controller.PatchBug(3, model).ExecuteAsync(CancellationToken.None).Result;

            //Assert

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void EditingExistingBug_ModelIsNullOrNotValid_ShouldReturn400_BadRequest()
        {
            var fakeBugs = new List<Bug>()
                {
                    //Arrange
                    new Bug()
                    {
                        Id = 1,
                        Title = "Bug #1",
                        Description = "Bug Description",
                        DateCreated = DateTime.Now.AddDays(-1)
                    },
                    new Bug()
                    {
                        Id = 1,
                        Title = "Bug #2",
                        Description = "Bug Description #2",
                        DateCreated = DateTime.Now.AddDays(-5)
                    }
                };

            var mockRepository = new Mock<IRepository<Bug>>();
            mockRepository
                .Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return fakeBugs.FirstOrDefault(f => f.Id == id);
                });

            var mockUnitOfWork = new Mock<IBugsUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Bugs).Returns(mockRepository.Object);

            EditBugBindingModel model = null;

            //Act

            var controller = new BugsController(mockUnitOfWork.Object);
            SetupController(controller);

            var response = controller.PatchBug(2, model).ExecuteAsync(CancellationToken.None).Result;

            //Assert

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private void SetupController(ApiController controller)
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }
    }
}
