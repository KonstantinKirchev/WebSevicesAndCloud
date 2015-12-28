namespace DistenceCalculatorRestful.Controllers
{
    using System;
    using System.Drawing;
    using System.Web.Http;
    

    public class ValuesController : ApiController
    {
        [HttpGet]
        public double CalculateDistance(int startX, int endX, int startY, int endY)
        {
            return Math.Sqrt(Math.Pow(startX - endX, 2) + Math.Pow(startY - endY, 2));
        }
    }
}
