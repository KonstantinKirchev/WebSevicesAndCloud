using System.Web.Http;
using Messages.Data.UnitOfWork;

namespace Messages.RestServices.Controllers
{
    public class BaseApiController : ApiController
    {
        //public BaseApiController()
        //{
        //    this.Data = new BugTrackerDbContext();
        //}

        //protected BugTrackerDbContext Data { get; set; }

        protected IMessagesUnitOfWork data;

        public BaseApiController(IMessagesUnitOfWork data)
        {
            this.data = data;
        }

        public BaseApiController()
            : this(new MessagesUnitOfWork())
        {
        }
    }
}