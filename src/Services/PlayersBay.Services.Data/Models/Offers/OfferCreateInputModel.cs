namespace PlayersBay.Services.Data.Models.Offers
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Data.Utilities;

    public class OfferCreateInputModel
    {
        [Display(Name = "Game")]
        public int GameId { get; set; }

        [Required]
        [Display(Name = "Offer Type")]
        public OfferType OfferType { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageUrl { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(DataConstants.Offer.MinPrice, DataConstants.Offer.MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(DataConstants.Offer.TitleMaxLength, MinimumLength = DataConstants.Offer.TitleMinLength, ErrorMessage = DataConstants.Offer.TitleLengthError)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(DataConstants.Offer.DescriptionMaxLength, MinimumLength = DataConstants.Offer.DescriptionMinLength, ErrorMessage = DataConstants.Offer.DescriptionLengthError)]
        public string Description { get; set; }

        [Display(Name = "Message to Buyer")]
        [DataType(DataType.MultilineText)]
        [StringLength(DataConstants.Offer.MessageToBuyerMaxLength, MinimumLength = DataConstants.Offer.MessageToBuyerMinLength, ErrorMessage = DataConstants.Offer.MessageToBuyerLengthError)]
        public string MessageToBuyer { get; set; }
    }
}
