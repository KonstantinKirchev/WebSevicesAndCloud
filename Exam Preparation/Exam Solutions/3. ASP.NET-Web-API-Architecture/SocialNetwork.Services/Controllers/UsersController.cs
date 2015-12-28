using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
//using System.Web.OData;
using SocialNetwork.Services.Models.BindingModels;
using SocialNetwork.Services.Models.ViewModels;

namespace SocialNetwork.Services.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        [Route("api/users/{username}/wall")]
        //[EnableQuery]
        [ResponseType(typeof(IQueryable<PostViewModel>))]      
        public IHttpActionResult GetUserWall(string username)
        {
            var user = this.Data.Users
                .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return this.BadRequest();
            }

            var userWall = this.Data.Posts
                .Where(u => u.WallOwnerId == user.Id)
                .Select(PostViewModel.Create);

            return this.Ok(userWall);
        }

        // GET api/users/search?name=...&minAge=...&maxAge=...&location=...
        [ActionName("search")]
        [HttpGet]
        public IHttpActionResult SearchUser([FromUri]UserSearchBindingModel model)
        {
            var userSearchResult = this.Data.Users.AsQueryable();

            if (model.Name != null)
            {
                userSearchResult = userSearchResult
                .Where(u => u.UserName.Contains(model.Name));
            }

            if (model.MinAge.HasValue)
            {
                userSearchResult = userSearchResult
                .Where(u => u.Age >= model.MinAge.Value);
            }

            if (model.MaxAge.HasValue)
            {
                userSearchResult = userSearchResult
                .Where(u => u.Age <= model.MaxAge.Value);
            }

            var finalResult = userSearchResult
                .Select(u => new
                {
                    u.UserName,
                    u.Age
                });
            return this.Ok(finalResult);
        }
    }
}
