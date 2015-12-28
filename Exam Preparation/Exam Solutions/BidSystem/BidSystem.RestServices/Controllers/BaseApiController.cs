using System.Web.Http;
using BidSystem.Data;
using BidSystem.Data.UnitOfWork;

namespace BidSystem.RestServices.Controllers
{
    public class BaseApiController : ApiController
    {
        //public BaseApiController()
        //{
        //    this.Data = new BidSystemDbContext();
        //}

        //protected BidSystemDbContext Data { get; set; }

        protected IBidSystemData data;

        public BaseApiController(IBidSystemData data)
        {
            this.data = data;
        }

        public BaseApiController()
            : this(new BidSystemData())
        {
        }
    }
}