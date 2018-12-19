namespace PlayersBay.Data.Models
{
    using System;

    using PlayersBay.Data.Common.Models;

    public class Message : BaseDeletableModel<int>
    {
        public string SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; }
        public virtual ApplicationUser Receiver { get; set; }

        public string Text { get; set; }

        public bool? IsRead { get; set; }
    }
}
