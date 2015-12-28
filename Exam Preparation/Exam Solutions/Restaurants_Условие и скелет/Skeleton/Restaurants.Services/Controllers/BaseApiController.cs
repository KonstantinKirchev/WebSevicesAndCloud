using System.Web.Http;
using Restaurants.Data;
using Restaurants.Data.UnitOfWork;

namespace Restaurants.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
        {
            this.Data = new RestaurantsContext();
        }

        protected RestaurantsContext Data { get; set; }

        //protected IRestaurantsUnitOfWork Data;

        //public BaseApiController(IRestaurantsUnitOfWork data)
        //{
        //    this.Data = data;
        //}

        //public BaseApiController()
        //    : this(new RestaurantsUnitOfWork())
        //{
        //}
    }
}