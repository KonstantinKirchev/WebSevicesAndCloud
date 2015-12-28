using System;
using System.ComponentModel.DataAnnotations;

namespace BidSystem.Data.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OfferId { get; set; }

        public virtual Offer Offer { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public string BidderId { get; set; }

        public virtual User Bidder { get; set; }

        public decimal OfferedPrice { get; set; }

        public string Comment { get; set; }
    }
}
