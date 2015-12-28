namespace MessagesRestService.Controllers
{
    using Messages.Data;
    using System.Web.Http;

    public abstract class BaseApiController : ApiController
    {
        private IMessagesData data;
        
        protected BaseApiController(IMessagesData data)
        {
            this.Data = data;
        }

        protected IMessagesData Data 
        { 
            get 
            {
                return this.data;
            }
            private set
            {
                this.data = value;
            }
        }
    }
}