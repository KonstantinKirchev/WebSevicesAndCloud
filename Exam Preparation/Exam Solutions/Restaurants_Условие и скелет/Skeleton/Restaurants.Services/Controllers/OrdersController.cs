using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Restaurants.Models;
using Restaurants.Services.Models.BindingModels;

namespace Restaurants.Services.Controllers
{
    public class OrdersController : BaseApiController
    {
        // GET: api/orders
        [HttpGet]
        [Route("api/orders")]
        public IHttpActionResult ViewPendingOrders
            ([FromUri] PendingOrderBindingModel model)
        {

            var currentUserId = this.User.Identity.GetUserId();

            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var orders = this.Data.Orders
                .Where(o => o.UserId == currentUserId && o.OrderStatus == OrderStatus.Pending)
                .OrderByDescending(o => o.CreatedOn)
                .Select(o => new
                {
                    Id = o.Id,
                    Meal = new
                    {
                        Id = o.Meal.Id,
                        Name = o.Meal.Name,
                        Price = o.Meal.Price,
                        Type = o.Meal.Type.Name
                    },
                    Quantity = o.Quantity,
                    Status = o.OrderStatus,
                    CreatedOn = o.CreatedOn

                });

            if (model != null)
            {
                var startPage = 0;
                int.TryParse(model.StartPage, out startPage);
                orders = orders.Skip(startPage);

                var limit = 0;
                int.TryParse(model.Limit, out limit);
                if (limit < 2 || limit > 10)
                {
                    return this.BadRequest("Limit should be in range[2...10]");
                }
                orders = orders.Take(limit);

                if (model.MealId != null)
                {
                    orders = orders.Where(o => o.Meal.Id == model.MealId);
                }

            }

            return this.Ok(orders);
        }
    }
}
