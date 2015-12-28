using System.Web.Http;
using Restaurants.Data;

namespace Restaurants.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
        {
            this.Data = new RestaurantsContext();
        }

        protected RestaurantsContext Data { get; set; }

        //protected IBugsUnitOfWork data;

        //public BaseApiController(IBugsUnitOfWork data)
        //{
        //    this.data = data;
        //}

        //public BaseApiController()
        //    : this(new BugsUnitOfWork())
        //{
        //}
    }
}