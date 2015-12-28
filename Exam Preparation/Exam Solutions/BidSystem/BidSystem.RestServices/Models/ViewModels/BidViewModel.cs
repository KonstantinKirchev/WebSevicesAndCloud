using System;
using System.Linq.Expressions;
using BidSystem.Data.Models;

namespace BidSystem.RestServices.Models.ViewModels
{
    public class BidViewModel
    {
        public int Id { get; set; }

        public int OfferId { get; set; }

        public DateTime DateCreated { get; set; }

        public string Bidder { get; set; }

        public decimal OfferedPrice { get; set; }

        public string Comment { get; set; }

        public static Expression<Func<Bid, BidViewModel>> Create()
        {
            return b => new BidViewModel
            {
                Id = b.Id,
                OfferId = b.OfferId,
                DateCreated = b.DateCreated,
                Bidder = b.Bidder.UserName,
                OfferedPrice = b.OfferedPrice,
                Comment = b.Comment
            };
        }
    }
}