using System;
using System.Linq;
using System.Web.Http;
using BidSystem.Data.UnitOfWork;
using BidSystem.RestServices.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace BidSystem.RestServices.Controllers
{
    [RoutePrefix("api/bids")]
    public class BidsController : BaseApiController
    {
         public BidsController(IBidSystemData data)
            : base(data)
        {
        }

         public BidsController()
        {
        }
        // GET: /api/bids/my
        [Route("my")]
        [HttpGet]
        public IHttpActionResult ListUserBids()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var user = this.data.Users.All().FirstOrDefault(u => u.Id == currentUserId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            var bids = this.data.Bids.All()
                .Where(b => b.BidderId == user.Id)
                .OrderByDescending(b => b.DateCreated)
                .Select(BidViewModel.Create());

            return this.Ok(bids);
        }

        // GET: /api/bids/won
        [Route("won")]
        [HttpGet]
        public IHttpActionResult ListUserWonBids()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var user = this.data.Users.All().FirstOrDefault(u => u.Id == currentUserId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            var bids = this.data.Bids.All()
                .Where(b => b.BidderId == user.Id && b.Offer.InitialPrice < b.OfferedPrice && b.Offer.ExpirationDateTime <= DateTime.Now)
                .OrderBy(b => b.DateCreated)
                .Select(BidViewModel.Create());

            return this.Ok(bids);
        }
    }
}