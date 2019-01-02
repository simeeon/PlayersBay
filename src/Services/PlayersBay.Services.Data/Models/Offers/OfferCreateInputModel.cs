namespace PlayersBay.Services.Data.Models.Offers
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Mapping;

    public class OfferCreateInputModel
    {
        [Required]
        [Display(Name = "Game")]
        public int GameId { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Offer Type")]
        public OfferType OfferType { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageUrl { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(Constants.Offer.MinPrice, Constants.Offer.MaxPrice)]
        public decimal Price { get; set; }

        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(Constants.Offer.DescriptionMaxLength, MinimumLength = Constants.Offer.DescriptionMinLength, ErrorMessage = Constants.Offer.DescriptionLengthError)]
        public string Description { get; set; }

        [Display(Name = "Message to Buyer")]
        [DataType(DataType.MultilineText)]
        [StringLength(Constants.Offer.MessageToBuyerMaxLength, MinimumLength = Constants.Offer.MessageToBuyerMinLength, ErrorMessage = Constants.Offer.MessageToBuyerLengthError)]
        public string MessageToBuyer { get; set; }
    }
}
