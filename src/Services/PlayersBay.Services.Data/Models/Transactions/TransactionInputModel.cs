namespace PlayersBay.Services.Data.Models.Transactions
{
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Mapping;

    public class TransactionInputModel : IMapTo<Transaction>
    {
        public string SellerName { get; set; }

        public string BuyerName { get; set; }

        public int OfferId { get; set; }
    }
}
