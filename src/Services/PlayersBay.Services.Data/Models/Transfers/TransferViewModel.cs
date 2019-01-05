namespace PlayersBay.Services.Data.Models.Transfers
{
    using System;

    using PlayersBay.Data.Models.Enums;

    public class TransferViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        public TransferType Type { get; set; }

        public TransferStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
