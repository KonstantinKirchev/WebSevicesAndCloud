using System;
using System.Linq;
using System.Web.Http;
using BugTracker.Data.Models;
using BugTracker.Data.UnitOfWork;
using BugTracker.RestServices.Infrastructure;
using BugTracker.RestServices.Models.BindingModels;
using BugTracker.RestServices.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace BugTracker.RestServices.Controllers
{
    [RoutePrefix("api")]
    public class CommentsController : BaseApiController
    {
        public CommentsController(IBugsUnitOfWork data)
            : base(data)
        {
        }

        public CommentsController()
        {
        }


        // GET api/comments
        [HttpGet]
        [Route("comments")]
        public IHttpActionResult GetAllComments()
        {
            var comments = this.Data.Comments.All()
                .OrderByDescending(c => c.DateCreated)
                .ThenByDescending(c => c.Id)
                .Select(c => new CommentWithBugViewModel()
                {
                    Id = c.Id,
                    Text = c.Text,
                    Author = c.Author == null ? null : c.Author.UserName,
                    DateCreated = c.DateCreated,
                    BugId = c.BugId,
                    BugTitle = c.Bug.Title
                });

            return this.Ok(comments);
        }

        //GET /api/bugs/{id}/comments
        [HttpGet]
        [Route("bugs/{id}/comments")]
        public IHttpActionResult GetCommentsForGivenBug(int id)
        {
            var bug = this.Data.Bugs.Find(id);

            if (bug == null)
            {
                return this.NotFound();
            }

            var comments = bug.Comments
                .OrderByDescending(c => c.DateCreated)
                .ThenByDescending(c => c.Id)
                .Select(c => new CommentViewModel 
                {
                    Id =c.Id,
                    Text = c.Text,
                    Author = c.Author == null ? null : c.Author.UserName,
                    DateCreated = c.DateCreated
                });

            return this.Ok(comments);
        }

        [HttpPost]
        [Route("bugs/{id}/comments")]
        public IHttpActionResult AddNewComment(int id, PostCommentBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model is empty");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var bug = this.Data.Bugs.Find(id);

            if (bug == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var user = this.Data.Users.Find(loggedUserId);

            var comment = new Comment()
            {
                Author = user,
                DateCreated = DateTime.Now,
                Text = model.Text
            };

            bug.Comments.Add(comment); // автоматично EF си сетва, че бъга който намерих по горе има коментар
            this.Data.SaveChanges();

            if (user != null)
            {
                return this.Ok(new {Id = bug.Id, Author = user.UserName, Message = "User comment added for bug #" + bug.Id});
            }

            return this.Ok(new { Id = bug.Id, Message = "Added anonymous comment for bug #" + bug.Id });
        }
    }

    

}