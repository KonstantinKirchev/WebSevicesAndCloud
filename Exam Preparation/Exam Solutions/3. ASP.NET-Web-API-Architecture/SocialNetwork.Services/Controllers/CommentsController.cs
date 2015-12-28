using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SocialNetwork.Models;
using SocialNetwork.Services.Models.BindingModels;

namespace SocialNetwork.Services.Controllers
{
    [Authorize]
    public class CommentsController : BaseApiController
    {
        [HttpPost]
        [Route("api/posts/{postId}/comments")]
        public IHttpActionResult AddCommentToPost(int postId, AddCommentsBindingModel model)
        {
            var post = this.Data.Posts.Find(postId);

            if (post == null)
            {
                return this.NotFound();
            }

            if (model == null)
            {
                return this.BadRequest("Empty model is not allowed.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userId = this.User.Identity.GetUserId();

            var comment = new Comment()
            {
                Content = model.Content,
                PostedOn = DateTime.Now,
                AuthorId = userId
            };

            this.Data.SaveChanges();

            return this.Ok(comment.Content);
        }

        // DELETE api/posts/{id}
        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(int id)
        {
            var post = this.Data.Posts.Find(id);

            if (post == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            
            if (loggedUserId != post.AuthorId && loggedUserId != post.WallOwnerId && !this.User.IsInRole("Admin"))
            {
                return this.Unauthorized();
            }

            this.Data.Posts.Remove(post);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
