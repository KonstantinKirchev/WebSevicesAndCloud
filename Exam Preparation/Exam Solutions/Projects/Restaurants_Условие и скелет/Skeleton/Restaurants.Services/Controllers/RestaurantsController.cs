using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Restaurants.Models;
using Restaurants.Services.Models.BindingModels;
using Restaurants.Services.Models.ViewModels;

namespace Restaurants.Services.Controllers
{
    [RoutePrefix("api/restaurants")]
    public class RestaurantsController : BaseApiController
    {
        // GET: api/Restaurants
        public IHttpActionResult GetRestaurants([FromUri]string townId = null)
        {
            var id = 0;
            if (townId != null)
            {       
                int.TryParse(townId, out id);
            }
            var restaurants = this.Data.Restaurants
                .Where(r => r.TownId == id)
                .OrderByDescending(r => r.Ratings.Select(rat => rat.Stars).Average())
                .ThenBy(r => r.Name)
                .Select(r => new 
                {
                    Id = r.Id,
                    Name = r.Name,
                    Rating = r.Ratings.Select(rat => rat.Stars),
                    Town = new TownViewModel
                    {
                        Id = r.TownId,
                        Name = r.Town.Name
                    }
                });

            return this.Ok(restaurants);
        }

        // GET: api/Restaurants/5
        [ResponseType(typeof(Restaurant))]
        [Route("{id}/meals")]
        public IHttpActionResult GetRestaurantMeals(int id)
        {
            var restaurant = this.Data.Restaurants.Find(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            var meals = this.Data.Meals
                .Where(m => m.RestaurantId == id)
                .OrderBy(m => m.Type.Order)
                .ThenBy(m => m.Name)
                .Select(m => new MealViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price,
                    Type = m.Type.Name
                });

            return Ok(meals);
        }

        // POST: api/Restaurants
        [ResponseType(typeof(Restaurant))]
        public IHttpActionResult PostRestaurant(RestaurantBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("The model is empty (need info for restaurant name and townId).");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = this.User.Identity.GetUserId();
            var user = this.Data.Users.FirstOrDefault(u => u.Id == currentUserId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            var restaurant = new Restaurant()
            {
                Name = model.Name,
                TownId = model.TownId
            };

            this.Data.Restaurants.Add(restaurant);
            this.Data.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = restaurant.Id }, new {Id = restaurant.Id, Name = restaurant.Name, Rating = restaurant.Ratings.Select(r=>r.Stars), Town = new {Id = restaurant.TownId, Name = restaurant.Town.Name}});
        }

        // POST: api/Restaurants
        [ResponseType(typeof(Restaurant))]
        [HttpPost]
        [Route("{id}/rate")]
        public IHttpActionResult RateAnExistingRestaurant(int id, RateAnExistingRestaurantBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("The rating is empty.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var restaurant = this.Data.Restaurants.Find(id);

            if (restaurant == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.Identity.GetUserId();
            var user = this.Data.Users.FirstOrDefault(u => u.Id == currentUserId);

            if (model.Stars < 1 || model.Stars > 10)
            {
                return this.BadRequest("The rating must be in the range[1...10]");
            }

            if (restaurant.OwnerId == currentUserId)
            {
                return this.BadRequest("The owner cannot rate his own restaurant");
            }

            if (user == null)
            {
                return this.Unauthorized();
            }

            var rating = new Rating()
            {
                Stars = model.Stars,
                RestaurantId = restaurant.Id,
                UserId = user.Id
            };

            var existingUser = this.Data.Ratings.FirstOrDefault(r => r.UserId == currentUserId);
            if (existingUser != null)
            {
                this.Data.Ratings.AddOrUpdate(rating);
                this.Data.SaveChanges();
            }
            else
            {
                this.Data.Ratings.Add(rating);
                this.Data.SaveChanges();
            }

            return this.Ok();
        }
    }
}