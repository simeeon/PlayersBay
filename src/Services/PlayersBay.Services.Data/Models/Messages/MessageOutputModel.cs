﻿namespace PlayersBay.Services.Data.Models.Messages
{
    using System;

    public class MessageOutputModel
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string Text { get; set; }

        public bool? IsRead { get; set; }
    }
}
