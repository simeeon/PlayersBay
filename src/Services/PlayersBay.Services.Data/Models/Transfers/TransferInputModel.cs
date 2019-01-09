namespace PlayersBay.Services.Data.Models.Transfers
{
    using System.ComponentModel.DataAnnotations;

    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Data.Utilities;

    public class TransferInputModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(DataConstants.Transfer.MinAmount, DataConstants.Transfer.MaxAmount)]
        public decimal Amount { get; set; }

        public TransferType Type { get; set; }

        public TransferStatus Status { get; set; }
    }
}
