using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using BugTracker.Data;
using BugTracker.RestServices.Models.ViewModels;
using BugTracker.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTracker.Tests.IntegrationTests
{
    [TestClass]
    public class BugCommentsIntegrationTests
    {
        [TestMethod]
        public void GetCommentsForExistingBug_ShouldReturn200Ok_TheBugWithComments()
        {
            // Arrange -> create a new bug with two comments
            TestingEngine.CleanDatabase();
            var context = new BugTrackerDbContext();

            var bugsToAdds = new BugModel[]
            {
                new BugModel() { Title = "First Bug" },
                new BugModel() { Title = "Second Bug", Description = "More info"},
                new BugModel() { Title = "Third Bug" }
            };

            // Act -> submit a few bugs
            foreach (var bug in bugsToAdds)
            {
                var httpPostResponse = TestingEngine.SubmitBugHttpPost(bug.Title, bug.Description);
                Thread.Sleep(2);

                // Assert -> ensure each bug is successfully submitted
                Assert.AreEqual(HttpStatusCode.Created, httpPostResponse.StatusCode);
            }

            var existingBug = context.Bugs.FirstOrDefault();

            if (existingBug == null)
            {
                Assert.Fail("Cannot perform tests - no bugs in DB.");
            }

            var endpoint = string.Format("api/bugs/{0}/comments", existingBug.Id);

            var httpResponse = TestingEngine.HttpClient.GetAsync(endpoint).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

            var comments = httpResponse.Content.ReadAsAsync<List<CommentViewModel>>().Result;

            foreach (var comment in comments)
            {
                Assert.IsNotNull(comment.Text);
                Assert.AreNotEqual(0, comment.Id);
            }

        }

        [TestMethod]
        public void GetCommentsForExistingBug_ShouldReturn404NotFound_NonExistingBug()
        {
            // Arrange -> create a new bug with two comments
            TestingEngine.CleanDatabase();
            var context = new BugTrackerDbContext();

            var bugsToAdds = new BugModel[]
            {
                new BugModel() { Title = "First Bug" },
                new BugModel() { Title = "Second Bug", Description = "More info"},
                new BugModel() { Title = "Third Bug" }
            };

            // Act -> submit a few bugs
            foreach (var bug in bugsToAdds)
            {
                var httpPostResponse = TestingEngine.SubmitBugHttpPost(bug.Title, bug.Description);
                Thread.Sleep(2);

                // Assert -> ensure each bug is successfully submitted
                Assert.AreEqual(HttpStatusCode.Created, httpPostResponse.StatusCode);
            }

            var existingBug = context.Bugs.FirstOrDefault();

            if (existingBug == null)
            {
                Assert.Fail("Cannot perform tests - no bugs in DB.");
            }

            var endpoint = ("api/bugs/-1/comments");

            var httpResponse = TestingEngine.HttpClient.GetAsync(endpoint).Result;

            Assert.AreEqual(HttpStatusCode.NotFound, httpResponse.StatusCode);

        }
    }
}
