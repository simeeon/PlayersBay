namespace PlayersBay.Services.Data.Models.Deals
{
    using System.ComponentModel.DataAnnotations;

    public class TopUpInputModel
    {
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 50000)]
        public decimal Amount { get; set; }
    }
}
