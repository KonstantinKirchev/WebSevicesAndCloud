using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Exercise_ASP.NET_Web_API.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        [Route("list")]
        [HttpGet]
        public string ListProduct()
        {
            return "product";
        }

        [Route("{id:int:min(10)}")]
        public string GetProducts(int id)
        {
            return "product " + id;
        }

        [Route("{name:maxlength(6)}")]
        public string GetProductsByName(string name)
        {
            return "product " + name;
        }

        [Route("{name}")]
        public HttpResponseMessage GetProductsResponse(string name)
        {
            var response = new HttpResponseMessage();

            if (name.StartsWith("Christmas"))
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.StatusCode = HttpStatusCode.Found;
            }
            return response;
        }

        [Route("sum")]
        public IHttpActionResult Sum(int a, int b)
        {
            if (a == 5)
            {
                return this.BadRequest("a cannot be 5");
            }
            return this.Ok(a + b);
        }
    }
}
