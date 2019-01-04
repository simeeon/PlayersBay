namespace PlayersBay.Services.Data.Models.Offers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    using PlayersBay.Common;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Mapping;

    public class OfferDetailsViewModel : IMapFrom<Offer>
    {
        public int Id { get; set; }

        [Display(Name = "Game")]
        public int GameId { get; set; }

        [Display(Name = "Seller")]
        public string SellerUsername { get; set; }

        [Display(Name = "Offer Type")]
        public string OfferType { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Expiry date")]
        public string OfferEnds => this.ExpiryDate.ToString(GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
