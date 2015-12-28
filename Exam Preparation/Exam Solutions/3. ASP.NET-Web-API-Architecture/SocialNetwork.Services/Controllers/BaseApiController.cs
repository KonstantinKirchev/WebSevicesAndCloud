using System.Web.Http;
using SocialNetwork.Data;

namespace SocialNetwork.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new SocialNetworkContext())
        {         
        }

        public BaseApiController(SocialNetworkContext data)
        {
            this.Data = data;
        }

        protected SocialNetworkContext Data { get; set; }

    }
}