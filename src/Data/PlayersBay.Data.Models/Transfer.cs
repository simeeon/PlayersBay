namespace PlayersBay.Data.Models
{

    using PlayersBay.Data.Common.Models;
    using PlayersBay.Data.Models.Enums;

    public class Transfer : BaseDeletableModel<int>
    {
        public TransferType Type { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public decimal Amount { get; set; }

        public TransferStatus Status { get; set; }
    }
}
