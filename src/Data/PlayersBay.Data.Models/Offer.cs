namespace PlayersBay.Data.Models
{
    using System;

    using PlayersBay.Data.Common.Models;
    using PlayersBay.Data.Models.Enums;

    public class Offer : BaseDeletableModel<int>
    {
        public Offer()
        {
            this.Feedback = new Feedback();
        }

        public string SellerId { get; set; }

        public ApplicationUser Seller { get; set; }

        public OfferType OfferType { get; set; }

        public string ImageUrl { get; set; }

        public DateTime ExpiryDate { get; set; }

        public OfferStatus Status { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string MessageToBuyer { get; set; }

        public int FeedbackId { get; set; }

        public virtual Feedback Feedback { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }
    }
}
