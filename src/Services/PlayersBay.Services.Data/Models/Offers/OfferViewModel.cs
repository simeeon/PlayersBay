namespace PlayersBay.Services.Data.Models.Offers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using PlayersBay.Common;
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Mapping;

    public class OfferViewModel : IMapFrom<Offer>
    {
        public int Id { get; set; }

        public string SellerId { get; set; }

        public ApplicationUser Seller { get; set; }

        [Display(Name = "Offer Type")]
        public OfferType OfferType { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Offer ends")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Created")]
        public string CreatedOnShort => this.CreatedOn.ToString(GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

        [Display(Name = "Expires")]
        public string ExpiryDateShort => this.ExpiryDate.ToString(GlobalConstants.DateFormat, CultureInfo.InvariantCulture);

        public OfferStatus Status { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        [Display(Name = "Title")]
        public string ShrotTitle => $"{this.Title.Substring(0, Math.Min(this.Title.Length, 25))}";

        public string Description { get; set; }

        [Display(Name = "Message To Buyer")]
        public string MessageToBuyer { get; set; }
    }
}
