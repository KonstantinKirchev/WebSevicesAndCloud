using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using OnlineShop.Models;
using OnlineShop.Services.Models.BindingModels;
using OnlineShop.Services.Models.ViewModels;

namespace OnlineShop.Services.Controllers
{
    [Authorize]
    public class AdsController : BaseApiController
    {
        // GET api/ads
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetAllAds()
        {
            var ads = this.Data.Ads.All()
                .Where(a => a.Status == 0)
                .OrderBy(a => a.Type)
                .ThenByDescending(a => a.PostedOn)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Description,
                    a.Price,
                    Owner = new
                    {
                        Id = a.OwnerId,
                        Username = a.Owner.UserName
                    },
                    Type = a.Type.Name,
                    a.PostedOn,
                    Categories = a.Categories.Select(c => new
                    {
                        c.Id,
                        c.Name
                    })
                });

            return this.Ok(ads);
        }

        // POST api/ads
        [HttpPost]
        public IHttpActionResult CreateAd(CreateAddBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var ad = new Ad()
            {
                Name = model.Name,
                Description = model.Description,
                TypeId = model.TypeId,
                Price = model.Price
            };

            this.Data.Ads.Add(ad);
            this.Data.SaveChanges();

            var result = this.Data.Ads.All()
                .Where(a => a.Id == ad.Id)
                .Select(AdViewModel.Create)
                .FirstOrDefault();

            return this.Ok(result);
        }

        [HttpPut]
        [Route("api/ads/{id}/close")]
        public IHttpActionResult CloseAd(int id, EditAdBindingModel model)
        {
            var ad = this.Data.Ads.Find(id);

            if (ad == null)
            {
                return this.NotFound();
            }

            string userId = this.User.Identity.GetUserId();

            if (ad.OwnerId != userId)
            {
                return this.Unauthorized();
            }

            ad.Status = model.Status;
            ad.ClosedOn = DateTime.Now;

            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
