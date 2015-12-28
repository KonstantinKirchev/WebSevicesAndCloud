using System;
using System.Linq;
using System.Threading;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SocialNetwork.Data;
using SocialNetwork.Models;
using SocialNetwork.Services.Models.BindingModels;
using SocialNetwork.Services.Models.ViewModels;

namespace SocialNetwork.Services.Controllers
{
    [Authorize]
    public class PostsController : BaseApiController
    {
        //public PostsController(SocialNetworkContext data)
        //    : base(data)
        //{
        //}
        // GET api/posts
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult GetPosts()
        {
            Thread.Sleep(5000);
            
            var data = this.Data.Posts
                .OrderBy(p => p.PostedOn)
                .Select(p=> new
                {
                    p.Id,
                    p.Content,
                    Author = p.Author.UserName,
                    Likes = p.Likes.Count
                });

            return this.Ok(data);
        }

        
        [HttpPost]
        public IHttpActionResult AddPost(AddPostBindingModel model)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            //if (loggedUserId == null)
            //{
            //    return this.Unauthorized();
            //}

            if (model == null)
            {
                return this.BadRequest("Model cannot be null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var wallOwner = this.Data.Users
                .FirstOrDefault(u => u.UserName == model.WallOwnerUsername);

            if (wallOwner == null)
            {
                return this.BadRequest(string.Format("User {0} does not exist.", model.WallOwnerUsername));
            }

            Post post = new Post()
            {
                AuthorId = loggedUserId,
                WallOwner = wallOwner,
                Content = model.Content,
                PostedOn = DateTime.Now
            };

            this.Data.Posts.Add(post);
            this.Data.SaveChanges();

            var data = this.Data.Posts
                .Where(p => p.Id == post.Id)
                .Select(PostViewModel.Create)
                .FirstOrDefault();

            return this.Ok(data);
        }

        // PUT api/posts/{id}
        [HttpPut]
        public IHttpActionResult EditPost(int id, [FromBody]EditPostBidingModel model)
        {
            var post = this.Data.Posts.Find(id);

            if (post == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();

            if (loggedUserId != post.AuthorId)
            {
                return this.Unauthorized();
            }

            if (model == null)
            {
                return this.BadRequest("Model is empty.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            post.Content = model.Content;
            this.Data.SaveChanges();

            var data = this.Data.Posts
                .Where(p => p.Id == post.Id)
                .Select(PostViewModel.Create)
                .FirstOrDefault();

            return this.Ok(data);
        }
    }
}