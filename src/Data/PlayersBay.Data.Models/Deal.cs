namespace PlayersBay.Data.Models
{
    using System;

    using PlayersBay.Data.Common.Models;

    public class Deal : BaseDeletableModel<int>
    {
        public string SellerId { get; set; }

        public virtual ApplicationUser Seller { get; set; }

        public string BuyerId { get; set; }

        public virtual ApplicationUser Buyer { get; set; }

        public int OfferId { get; set; }

        public Offer Offer { get; set; }
    }
}
