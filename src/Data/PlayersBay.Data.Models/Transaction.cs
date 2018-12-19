namespace PlayersBay.Data.Models
{
    using System;

    using PlayersBay.Data.Common.Models;

    public class Transaction : BaseDeletableModel<int>
    {
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }

        public int OfferId { get; set; }

        public Offer Offer { get; set; }
    }
}
