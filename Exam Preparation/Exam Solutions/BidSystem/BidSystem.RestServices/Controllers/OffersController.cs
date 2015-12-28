using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using BidSystem.Data.Models;
using BidSystem.Data.UnitOfWork;
using BidSystem.RestServices.Models.BindingModels;
using BidSystem.RestServices.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace BidSystem.RestServices.Controllers
{
    [RoutePrefix("api/offers")]
    public class OffersController : BaseApiController
    {
        public OffersController(IBidSystemData data)
            : base(data)
        {
        }

        public OffersController()
        {
        }
        // GET: api/Offers/all
        [Route("all")]
        public IHttpActionResult GetOffers()
        {
            var offers = this.data.Offers.All()
                .OrderByDescending(o => o.DatePublished)
                .Select(OfferViewModel.Create);

            return this.Ok(offers);
        }

        // GET: api/Offers/active
        [Route("active")]
        public IHttpActionResult GetActiveOffers()
        {
            var offers = this.data.Offers.All()
                .Where(o => o.ExpirationDateTime > DateTime.Now)
                .OrderBy(o => o.ExpirationDateTime)
                .Select(OfferViewModel.Create);

            return this.Ok(offers);
        }

        // GET: api/Offers/expired
        [Route("expired")]
        public IHttpActionResult GetExpiredOffers()
        {
            var offers = this.data.Offers.All()
                .Where(o => o.ExpirationDateTime <= DateTime.Now)
                .OrderBy(o => o.ExpirationDateTime)
                .Select(OfferViewModel.Create);

            return this.Ok(offers);
        }

        // GET /api/offers/details/{id}
        [ResponseType(typeof(Offer))]
        [Route("details/{id}")]
        public IHttpActionResult GetOffer(int id)
        {
            var offer = this.data.Offers.All()
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Seller = o.Seller == null ? null : o.Seller.UserName,
                    DatePublished = o.DatePublished,
                    InitialPrice = o.InitialPrice,
                    ExpirationDateTime = o.ExpirationDateTime,
                    IsExpired = o.ExpirationDateTime <= DateTime.Now,
                    BidWinner = o.ExpirationDateTime <= DateTime.Now && o.Bids.Count > 0
                        ? o.Bids.OrderByDescending(b => b.OfferedPrice).FirstOrDefault().Bidder.UserName
                        : null,
                    Bids = o.Bids
                       .OrderByDescending(b => b.Id)
                       .Select(b => new
                    {
                        b.Id,
                        b.OfferId,
                        b.DateCreated,
                        b.Bidder.UserName,
                        b.OfferedPrice,
                        b.Comment
                    })

                })
                .FirstOrDefault();

            if (offer == null)
            {
                return NotFound();
            }

            return Ok(offer);
        }

        // GET: /api/offers/my
        [Route("my")]
        [HttpGet]
        public IHttpActionResult ListUserOffers()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var user = this.data.Users.All().FirstOrDefault(u => u.Id == currentUserId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            var offers = this.data.Offers.All()
                .Where(o => o.SellerId == user.Id)
                .OrderBy(o => o.DatePublished)
                .Select(OfferViewModel.Create);

            return this.Ok(offers);
        }

        // POST: api/Offers
        [ResponseType(typeof(Offer))]
        public IHttpActionResult PostOffer(OfferBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Offer cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUserId = this.User.Identity.GetUserId();
            var user = this.data.Users.All().FirstOrDefault(u => u.Id == currentUserId);

            if (user == null)
            {
                return this.Unauthorized();
            }
            
            var offer = new Offer()
            {
                Title = model.Title,
                Description = model.Description,
                ExpirationDateTime = model.ExpirationDateTime,
                InitialPrice = model.InitialPrice,
                SellerId = currentUserId,
                DatePublished = DateTime.Now
            };

            this.data.Offers.Add(offer);
            this.data.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = offer.Id }, new { Id = offer.Id, Seller = this.User.Identity.GetUserName(), Message = "Offer created." });
        }

        // POST: /api/offers/117/bid
        [Route("{id}/bid")]
        [HttpPost]
        public IHttpActionResult BidForExistingOffer(int id, BidBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Bid cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUserId = this.User.Identity.GetUserId();
            var user = this.data.Users.All().FirstOrDefault(u => u.Id == currentUserId);
            //var user = this.Data.UserStore.FindByIdAsync(currentUserId).Result;

            if (user == null)
            {
                return this.Unauthorized();
            }

            var offer = this.data.Offers.All()
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    Id = o.Id,
                    InitialPrice = o.InitialPrice,
                    ExpirationDateTime = o.ExpirationDateTime,
                    Bids = o.Bids.Select(b => b.OfferedPrice)
                })
                .FirstOrDefault();

            if (offer == null)
            {
                return NotFound();
            }

            if (offer.ExpirationDateTime < DateTime.Now)
            {
                return this.BadRequest("Offer has expired.");
            }

            var maxBidPrice = offer.InitialPrice;
            if (offer.Bids.Any())
            {
                maxBidPrice = offer.Bids.Max();
            }

            if (model.BidPrice <= maxBidPrice)
            {
                return this.BadRequest("Your bid should be > " + maxBidPrice);
            }

            var bid = new Bid
            {
                OfferedPrice = model.BidPrice,
                Comment = model.Comment,
                DateCreated = DateTime.Now,
                OfferId = id,
                BidderId = currentUserId
            };

            this.data.Bids.Add(bid);
            this.data.SaveChanges();

            return this.Ok(
                new { bid.Id, Bidder = user.UserName, Message = "Bid created." });
        }
    }
}