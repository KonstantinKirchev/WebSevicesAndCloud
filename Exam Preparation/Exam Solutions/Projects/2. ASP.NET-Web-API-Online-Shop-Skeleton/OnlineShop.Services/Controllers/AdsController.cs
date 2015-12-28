using System;
using System.Linq;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services.Models;

namespace OnlineShop.Services.Controllers
{
    [Authorize]
    public class AdsController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetAds()
        {
            var ads = this.Data.Ads
                .OrderBy(a => a.Type)
                .ThenBy(a => a.PostedOn)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Description,
                    a.Price,
                    owner = new
                    {
                        id = a.Owner.Id,
                        username = a.Owner.UserName
                    },
                    a.Type,
                    a.PostedOn,
                    categories = a.Categories.Select(s => new
                    {
                        s.Id,
                        s.Name
                    })
                });
            return this.Ok();
        }

        [HttpPost]
        public IHttpActionResult CreateAd(CreateAdBindingModel model)
        {

            if (model == null)
            {
                return this.BadRequest("Model cannot be null (no data in request)");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var ad = new Ad()
            {
                Id = 12,
                Name = "Petar",
                Description = "What's up!",
                TypeId = 1,
                Price = 12.99m
            };

            this.Data.Ads.Add(ad);
            this.Data.SaveChanges();

            var result = this.Data.Ads
                .Where(a => a.Id == ad.Id)
                .Select(a => AdViewModel.Create)
                .FirstOrDefault();

            return this.Ok(result);
        }

        [Route("api/ads/{id}/close")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult CloseAd(int id)
        {
            Ad ad = this.Data.Ads
                .FirstOrDefault(a => a.Id == id);
            if (ad == null)
            {
                return this.NotFound();
            }

            var userId = this.User.Identity.GetUserId();

            if (ad.OwnerId != userId)
            {
                return this.Unauthorized();
            }
            return this.Ok();

        }
    }
}