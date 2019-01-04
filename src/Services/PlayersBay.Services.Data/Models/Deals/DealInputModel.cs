namespace PlayersBay.Services.Data.Models.Deals
{
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Mapping;

    public class DealInputModel : IMapTo<Deal>
    {
        public string SellerName { get; set; }

        public string BuyerName { get; set; }

        public int OfferId { get; set; }
    }
}
