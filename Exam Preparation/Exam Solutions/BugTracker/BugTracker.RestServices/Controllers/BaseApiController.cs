using System.Web.Http;
using BugTracker.Data.UnitOfWork;
using BugTracker.RestServices.Infrastructure;

namespace BugTracker.RestServices.Controllers
{
    public class BaseApiController : ApiController
    {
        //public BaseApiController()
        //{
        //    this.data = new BugTrackerDbContext();
        //}

        //protected BugTrackerDbContext data { get; set; }

        protected IBugsUnitOfWork Data { get; set; }

        public BaseApiController(IBugsUnitOfWork data)
        {
            this.Data = data;
        }

        public BaseApiController()
            : this(new BugsUnitOfWork())
        {
        }
    }
}