namespace PlayersBay.Services.Data.Models.Transfers
{
    using System.ComponentModel.DataAnnotations;

    using PlayersBay.Data.Models.Enums;

    public class WithdrawalInputModel
    {
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 50000)]
        public decimal Amount { get; set; }

        public TransferType Type { get; set; }

        public TransferStatus Status { get; set; }
    }
}
