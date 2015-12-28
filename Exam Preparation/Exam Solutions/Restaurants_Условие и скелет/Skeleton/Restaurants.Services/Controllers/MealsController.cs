using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Restaurants.Data.UnitOfWork;
using Restaurants.Models;
using Restaurants.Services.Models.BindingModels;

namespace Restaurants.Services.Controllers
{
    [RoutePrefix("api/meals")]
    public class MealsController : BaseApiController
    {
        //public MealsController(IRestaurantsUnitOfWork data)
        //    : base(data)
        //{
        //}

        //public MealsController()
        //{
        //}

        // PUT: api/Meals/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult EditExistingMeal(int id, EditMealBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("The model is empty.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var meal = this.Data.Meals.Find(id);
            var existingType = this.Data.MealTypes.FirstOrDefault(m => m.Id == model.TypeId);

            if (meal == null)
            {
                return this.NotFound();
            }

            if (existingType == null)
            {
                return this.BadRequest("This type of meal does not exist.");
            }

            var currentUserId = this.User.Identity.GetUserId();
            var user = this.Data.Users.FirstOrDefault(u => u.Id == currentUserId);

            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var restaurant = this.Data.Restaurants.FirstOrDefault(r => r.OwnerId == user.Id);
            
            if (restaurant != null)
            {
                return this.Unauthorized();
            }

            meal.Name = model.Name;
            meal.Price = model.Price;
            meal.TypeId = model.TypeId;
            this.Data.SaveChanges();

            return this.Ok(new
            {
                Id = meal.Id,
                Name = meal.Name,
                Price = meal.Price,
                Type = existingType.Name
            });
        }

        // POST: api/Meals
        [ResponseType(typeof(Meal))]
        [HttpPost]
        public IHttpActionResult CreateNewMeal(MealBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("The model is empty.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRestaurant = this.Data.Restaurants.FirstOrDefault(r => r.Id == model.RestaurantId);
            var typeMeal = this.Data.Meals.FirstOrDefault(m => m.TypeId == model.TypeId);

            if (existingRestaurant == null)
            {
                return this.BadRequest("The restaurant does not exist.");
            }

            if (typeMeal == null)
            {
                return this.BadRequest("This type of meal does not exist.");
            }

            var meal = new Meal()
            {
                Name = model.Name,
                Price = model.Price,
                TypeId = model.TypeId,
                RestaurantId = model.RestaurantId
            };

            this.Data.Meals.Add(meal);
            this.Data.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = meal.Id }, new {Id = meal.Id, Name = meal.Name, Price = meal.Price, Type = typeMeal.Name});
        }

        // POST: api/Meals
        [ResponseType(typeof(Meal))]
        [HttpPost]
        [Route("{id}/order")]
        public IHttpActionResult CreateNewOrder(int id, OrderBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("The model is empty.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = this.User.Identity.GetUserId();

            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var meal = this.Data.Meals.FirstOrDefault(m => m.Id == id);

            if (meal == null)
            {
                return this.NotFound();
            }

            var order = new Order()
            {
                Quantity = model.Quantity,
                MealId = meal.Id,
                UserId = currentUserId,
                OrderStatus = OrderStatus.Pending,
                CreatedOn = DateTime.Now
            };
            

            this.Data.Orders.Add(order);
            this.Data.SaveChanges();

            return this.Ok();
        }

        // DELETE: api/Meals/5
        [ResponseType(typeof(Meal))]
        [Route("{id}")]
        public IHttpActionResult DeleteMeal(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var restaurantOwner = this.Data.Restaurants.FirstOrDefault(r => r.OwnerId == currentUserId);

            if (restaurantOwner == null)
            {
                return this.Unauthorized();
            }

            var meal = this.Data.Meals.Find(id);

            if (meal == null)
            {
                return NotFound();
            }

            this.Data.Meals.Remove(meal);
            this.Data.SaveChanges();

            return Ok(new { Message = "Meal #" + meal.Id + " deleted."});
        }
    }
}