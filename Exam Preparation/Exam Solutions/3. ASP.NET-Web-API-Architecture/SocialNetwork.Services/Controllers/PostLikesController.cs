using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SocialNetwork.Models;

namespace SocialNetwork.Services.Controllers
{
    [Authorize]
    public class PostLikesController : BaseApiController
    {
        //POST api/posts/{postId}/likes
        [Route("api/posts/{id}/likes"), HttpPost]
        public IHttpActionResult LikePost(int id)
        {
            var post = this.Data.Posts.Find(id);
            
            if (post == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();

            var isAlreadyLiked = post.Likes
                .Any(p => p.UserId == loggedUserId);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this post.");
            }

            if (loggedUserId == post.AuthorId)
            {
                return this.BadRequest("Cannot like his own posts.");
            }

            post.Likes.Add(new PostLike()
            {
                UserId = loggedUserId
            });

            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
